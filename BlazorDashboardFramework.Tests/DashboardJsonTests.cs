using System.Text.Json;
using System.Text.Json.Nodes;
using BlazorDashboardFramework;

namespace BlazorDashboardFramework.Tests;

public class DashboardJsonTests
{
    private static string Fixture(string name) =>
        File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Fixtures", name));

    [Fact]
    public void Summary_fixture_loads_with_all_rows_and_columns()
    {
        var dashboard = Dashboard.FromJson(Fixture("adf-summary-dashboard.json"));

        Assert.NotNull(dashboard);
        Assert.Equal("Summary", dashboard!.Title);
        Assert.Equal("12/6-6/4-4-4/3-3-3-3/2-2-2-2-2-2", dashboard.Layout);
        Assert.Equal("./src/templates/dashboard-title.html", dashboard.TitleTemplateUrl);
        Assert.Equal(new[] { 1, 2, 3, 4, 6 }, dashboard.Rows.Select(r => r.Columns.Count));
        Assert.Equal(new[] { "col-md-12", "col-md-6", "col-md-4", "col-md-3", "col-md-2" },
            dashboard.Rows.Select(r => r.Columns[0].StyleClass));
        Assert.Equal("1603122708579-74", dashboard.Rows[0].Columns[0].ColumnId);

        var widget = Assert.Single(dashboard.EnumerateWidgets());
        Assert.Equal("1603122715475-90", widget.WidgetInstanceId);
        Assert.Equal("pieceCounter.workSessions", widget.Type);
        Assert.Equal("Efficiency by Employee", widget.Title);
        Assert.Equal("./src/templates/widget-edit.html", widget.EditTemplateUrl);
        Assert.Equal("./src/templates/widget-title.html", widget.TitleTemplateUrl);
        Assert.Equal(75, widget.Config!.Value.GetProperty("cutoutPercentage").GetInt32());
        Assert.Equal(43, widget.Config!.Value.GetProperty("feedbackColorSchemeId").GetInt32());
    }

    [Fact]
    public void Summary_fixture_round_trips_exactly()
    {
        var original = Fixture("adf-summary-dashboard.json");
        var saved = Dashboard.FromJson(original)!.ToJson();

        Assert.True(JsonNode.DeepEquals(JsonNode.Parse(original), JsonNode.Parse(saved)),
            $"Round-trip changed the JSON:{Environment.NewLine}{saved}");
    }

    [Fact]
    public void Summary_fixture_survives_two_round_trips_and_deep_copy()
    {
        var original = JsonNode.Parse(Fixture("adf-summary-dashboard.json"));
        var once = Dashboard.FromJson(original!.ToJsonString())!;
        var copy = once.DeepCopy()!;
        var twice = Dashboard.FromJson(copy.ToJson())!.ToJson();

        Assert.True(JsonNode.DeepEquals(original, JsonNode.Parse(twice)));
    }

    [Fact]
    public void Nested_fixture_loads_tolerantly()
    {
        var dashboard = Dashboard.FromJson(Fixture("adf-nested-with-extras.json"))!;

        var outer = dashboard.Rows[0];
        Assert.Equal("my-row", outer.StyleClass);
        Assert.Equal("1543612345678", outer.Columns[0].ColumnId);          // numeric id read as string
        Assert.Equal("1543612345679", outer.Columns[0].Widgets[0].WidgetInstanceId);

        var container = outer.Columns[1];
        Assert.Equal(2, container.Rows.Count);
        var innerColumns = container.Rows[1].Columns;
        Assert.Empty(innerColumns[0].Widgets);                              // "widgets": null
        Assert.Equal(string.Empty, innerColumns[1].StyleClass);             // styleClass missing
        Assert.Equal(string.Empty, innerColumns[1].Widgets[0].Title);       // title missing
        Assert.Empty(innerColumns[2].Rows);                                 // "rows": null

        var iframe = outer.Columns[0].Widgets[2];                           // config stored as a string
        Assert.Equal(JsonValueKind.Object, iframe.Config!.Value.ValueKind);
        Assert.Equal("420px", iframe.Config.Value.GetProperty("height").GetString());
    }

    [Fact]
    public void Nested_fixture_keeps_unknown_keys_on_save()
    {
        var saved = JsonNode.Parse(Dashboard.FromJson(Fixture("adf-nested-with-extras.json"))!.ToJson())!;

        Assert.True(saved["editable"]!.GetValue<bool>());
        Assert.Equal("object:1", saved["$$hashKey"]!.GetValue<string>());
        var row = saved["rows"]![0]!;
        Assert.Equal("object:20", row["$$hashKey"]!.GetValue<string>());
        Assert.Equal("my-row", row["styleClass"]!.GetValue<string>());
        Assert.Null(row["StyleClass"]);
        var column = row["columns"]![0]!;
        Assert.Equal("object:25", column["$$hashKey"]!.GetValue<string>());
        var clock = column["widgets"]![0]!;
        Assert.Equal("object:40", clock["$$hashKey"]!.GetValue<string>());
        Assert.Equal("panel-info", clock["styleClass"]!.GetValue<string>());
        Assert.False(clock["frameless"]!.GetValue<bool>());
        var links = column["widgets"]![1]!["config"]!["links"]!.AsArray();
        Assert.Equal("https://www.piececounter.com", links[0]!["href"]!.GetValue<string>());
        var unsupported = row["columns"]![1]!["rows"]![1]!["columns"]![1]!["widgets"]![0]!["config"]!;
        Assert.Equal(250.5, unsupported["maxWidth"]!.GetValue<double>());
        Assert.Equal(0, unsupported["minValue"]!.GetValue<int>());
    }

    [Fact]
    public void Leaf_columns_do_not_write_rows_and_containers_do_not_write_widgets()
    {
        var saved = JsonNode.Parse(Dashboard.FromJson(Fixture("adf-nested-with-extras.json"))!.ToJson())!;
        var leaf = saved["rows"]![0]!["columns"]![0]!.AsObject();
        var container = saved["rows"]![0]!["columns"]![1]!.AsObject();

        Assert.False(leaf.ContainsKey("rows"));
        Assert.True(leaf.ContainsKey("widgets"));
        Assert.False(container.ContainsKey("widgets"));
        Assert.True(container.ContainsKey("rows"));
    }

    [Fact]
    public void Empty_or_invalid_input()
    {
        Assert.Null(Dashboard.FromJson(null));
        Assert.Null(Dashboard.FromJson("  "));
        var empty = Dashboard.FromJson("{}")!;
        Assert.Empty(empty.Rows);
        var nullRows = Dashboard.FromJson("""{"rows":null,"title":null}""")!;
        Assert.Empty(nullRows.Rows);
    }
}
