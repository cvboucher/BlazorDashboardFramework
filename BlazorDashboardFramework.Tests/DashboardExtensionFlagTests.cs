namespace BlazorDashboardFramework.Tests;

public class DashboardExtensionFlagTests
{
    [Fact]
    public void Missing_flag_is_false()
    {
        Assert.False(new Dashboard().GetExtensionFlag("hideDatesAndDeadlines"));
    }

    [Fact]
    public void Reads_flag_saved_by_angular_dashboard_framework()
    {
        var dashboard = Dashboard.FromJson("""{"title":"Home","structure":"6-6","rows":[],"hideDatesAndDeadlines":true}""")!;
        Assert.True(dashboard.GetExtensionFlag("hideDatesAndDeadlines"));
    }

    [Fact]
    public void Non_boolean_value_is_false()
    {
        var dashboard = Dashboard.FromJson("""{"title":"Home","structure":"6-6","rows":[],"hideDatesAndDeadlines":"yes"}""")!;
        Assert.False(dashboard.GetExtensionFlag("hideDatesAndDeadlines"));
    }

    [Fact]
    public void Set_flag_round_trips_through_json()
    {
        var dashboard = new Dashboard { Title = "Home" };
        dashboard.SetExtensionFlag("hideDatesAndDeadlines", true);
        var reloaded = Dashboard.FromJson(dashboard.ToJson())!;
        Assert.True(reloaded.GetExtensionFlag("hideDatesAndDeadlines"));
        reloaded.SetExtensionFlag("hideDatesAndDeadlines", false);
        Assert.False(Dashboard.FromJson(reloaded.ToJson())!.GetExtensionFlag("hideDatesAndDeadlines"));
    }

    [Fact]
    public void Other_extension_data_is_preserved_when_setting_a_flag()
    {
        var dashboard = Dashboard.FromJson("""{"title":"Home","structure":"6-6","rows":[],"editable":true}""")!;
        dashboard.SetExtensionFlag("hideDatesAndDeadlines", true);
        var json = dashboard.ToJson();
        Assert.Contains("\"editable\":true", json);
        Assert.Contains("\"hideDatesAndDeadlines\":true", json);
    }
}
