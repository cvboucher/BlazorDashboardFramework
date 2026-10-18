using System.Text.Json;
using System.Text.Json.Nodes;
using BlazorDashboardFramework;

namespace BlazorDashboardFramework.Tests;

public class TestConfig
{
    public string? PrimaryMetric { get; set; } = "efficiency";
    public int PrimaryMetricDecimalPlaces { get; set; } = 3;
    public int? MaxRows { get; set; }
    public int MaxWidth { get; set; } = 300;
    public double? MaxValue { get; set; }
    public bool Stacked { get; set; }
    public int[]? StoreIds { get; set; }
    public string? Note { get; set; }
}

public class WidgetConfigTests
{
    private static WidgetInstance Widget(string configJson) => new()
    {
        Type = "test",
        Title = "Test",
        Config = JsonDocument.Parse(configJson).RootElement.Clone(),
    };

    [Fact]
    public void Reads_normal_config()
    {
        var config = Widget("""{"primaryMetric":"pieces","primaryMetricDecimalPlaces":1,"maxRows":20,"storeIds":[1,2]}""")
            .GetConfig<TestConfig>();

        Assert.Equal("pieces", config.PrimaryMetric);
        Assert.Equal(1, config.PrimaryMetricDecimalPlaces);
        Assert.Equal(20, config.MaxRows);
        Assert.Equal(new[] { 1, 2 }, config.StoreIds);
        Assert.Equal(300, config.MaxWidth);
    }

    [Fact]
    public void Null_and_string_and_fraction_values_do_not_break_the_config()
    {
        var config = Widget("""
            {"primaryMetric":"pph","primaryMetricDecimalPlaces":null,"maxRows":"10","maxWidth":250.5,
             "maxValue":"","stacked":"true","storeIds":["3",4],"note":12}
            """).GetConfig<TestConfig>();

        Assert.Equal("pph", config.PrimaryMetric);
        Assert.Equal(0, config.PrimaryMetricDecimalPlaces);
        Assert.Equal(10, config.MaxRows);
        Assert.Equal(251, config.MaxWidth);
        Assert.Null(config.MaxValue);
        Assert.True(config.Stacked);
        Assert.Equal(new[] { 3, 4 }, config.StoreIds);
        Assert.Equal("12", config.Note);
    }

    [Fact]
    public void A_bad_property_is_skipped_not_the_whole_config()
    {
        var config = Widget("""{"primaryMetric":"hours","storeIds":{"oops":1},"maxRows":7}""")
            .GetConfig<TestConfig>();

        Assert.Equal("hours", config.PrimaryMetric);
        Assert.Equal(7, config.MaxRows);
        Assert.Null(config.StoreIds);
    }

    [Fact]
    public void Missing_or_non_object_config_gives_defaults()
    {
        Assert.Equal(3, new WidgetInstance { Type = "t" }.GetConfig<TestConfig>().PrimaryMetricDecimalPlaces);
        Assert.Equal(3, Widget("\"not json\"").GetConfig<TestConfig>().PrimaryMetricDecimalPlaces);
        Assert.Equal(3, Widget("[1,2]").GetConfig<TestConfig>().PrimaryMetricDecimalPlaces);
        Assert.IsType<TestConfig>(Widget("{}").GetConfig(typeof(TestConfig)));
    }

    [Fact]
    public void SetConfig_merges_and_keeps_unknown_keys()
    {
        var widget = Widget("""{"primaryMetric":"pieces","minValue":0,"links":[{"title":"a"}],"MaxRows":5}""");
        var config = widget.GetConfig<TestConfig>();
        config.PrimaryMetric = "efficiency";
        config.MaxRows = 12;

        widget.SetConfig(config);
        var saved = JsonNode.Parse(widget.Config!.Value.GetRawText())!.AsObject();

        Assert.Equal("efficiency", saved["primaryMetric"]!.GetValue<string>());
        Assert.Equal(0, saved["minValue"]!.GetValue<int>());
        Assert.Equal("a", saved["links"]![0]!["title"]!.GetValue<string>());
        Assert.Equal(12, saved["MaxRows"]!.GetValue<int>());   // original key casing kept
        Assert.False(saved.ContainsKey("maxRows"));
        Assert.False(saved.ContainsKey("note"));               // null values not added
        Assert.Equal(300, saved["maxWidth"]!.GetValue<int>());
    }

    [Fact]
    public void SetConfig_raises_ConfigChanged()
    {
        var widget = Widget("{}");
        var raised = false;
        widget.ConfigChanged += (_, _) => raised = true;
        widget.SetConfig(new TestConfig());
        Assert.True(raised);
    }
}
