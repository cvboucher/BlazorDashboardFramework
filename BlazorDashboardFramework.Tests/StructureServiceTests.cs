using BlazorDashboardFramework.Services;

namespace BlazorDashboardFramework.Tests;

public class StructureServiceTests
{
    private static IEnumerable<string> Shape(IEnumerable<Row> rows) =>
        rows.Select(r => string.Join(",", r.Columns.Select(c =>
            c.StyleClass + (c.Rows.Count > 0 ? "[" + string.Join("|", Shape(c.Rows)) + "]" : ""))));

    [Theory]
    [InlineData("6-6")]
    [InlineData("4-8")]
    [InlineData("12/4-4-4")]
    [InlineData("12/6-6")]
    [InlineData("12/6-6/12")]
    [InlineData("3-9 (12/6-6)")]
    [InlineData("12/3-3-3-3")]
    [InlineData("3-9 (12/4-4-4)")]
    [InlineData("12/3-6-3/12")]
    [InlineData("12/6-6/4-4-4/3-3-3-3/2-2-2-2-2-2")]
    [InlineData("2-10 (12/6-6/4-4-4/3-3-3-3)")]
    [InlineData("3-9 (12/3-3-3-3)")]
    public void All_ADF_structures_exist(string name)
    {
        var service = new StructureService();
        var layout = service.GetLayout(name);

        Assert.NotNull(layout);
        Assert.Equal(name, layout!.Layout);
        Assert.NotEmpty(layout.Rows);
    }

    [Fact]
    public void Layout_copies_get_unique_column_ids()
    {
        var service = new StructureService();
        var a = service.GetLayout("3-9 (12/4-4-4)")!;
        var b = service.GetLayout("3-9 (12/4-4-4)")!;

        var idsA = AllColumns(a.Rows).Select(c => c.ColumnId).ToList();
        var idsB = AllColumns(b.Rows).Select(c => c.ColumnId).ToList();
        Assert.Equal(idsA.Count, idsA.Distinct().Count());
        Assert.Empty(idsA.Intersect(idsB));
    }

    [Fact]
    public void Default_layout_is_3_9()
    {
        var layout = new StructureService().GetDefaultLayout();
        Assert.Equal("3-9 (12/4-4-4)", layout.Layout);
    }

    [Fact]
    public void Summary_fixture_matches_its_structure()
    {
        var json = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Fixtures", "adf-summary-dashboard.json"));
        var dashboard = Dashboard.FromJson(json)!;
        var structure = new StructureService().GetLayout(dashboard.Layout)!;

        Assert.Equal(Shape(structure.Rows), Shape(dashboard.Rows));
    }

    [Fact]
    public void Changing_structure_keeps_all_widgets()
    {
        var json = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Fixtures", "adf-summary-dashboard.json"));
        var dashboard = Dashboard.FromJson(json)!;
        var before = dashboard.EnumerateWidgets().Select(w => w.WidgetInstanceId).ToList();

        var layout = new StructureService().GetLayout("6-6")!;
        layout.PopulateWidgets(dashboard.GetAllWidgets());

        Assert.Equal(before, layout.EnumerateWidgets().Select(w => w.WidgetInstanceId).ToList());
    }

    private static IEnumerable<Column> AllColumns(IEnumerable<Row> rows) =>
        rows.SelectMany(r => r.Columns).SelectMany(c => new[] { c }.Concat(AllColumns(c.Rows)));
}
