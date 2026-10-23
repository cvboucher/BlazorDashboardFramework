using System.Globalization;
using System.Text.Json;
using BlazorDashboardFramework.Widgets.Clock;

namespace BlazorDashboardFramework.Tests;

public class ClockTests
{
    private static readonly CultureInfo En = CultureInfo.GetCultureInfo("en-US");
    private static readonly DateTimeOffset Sample = new(2026, 10, 3, 14, 5, 9, 45, TimeSpan.FromHours(-7));

    [Theory]
    [InlineData("HH:mm:ss", "14:05:09")]
    [InlineData("YYYY-MM-DD", "2026-10-03")]
    [InlineData("h:mm A", "2:05 PM")]
    [InlineData("hh:mm a", "02:05 pm")]
    [InlineData("dddd, MMMM Do YYYY", "Saturday, October 3rd 2026")]
    [InlineData("ddd MMM D", "Sat Oct 3")]
    [InlineData("[Week] W, Q[Q]", "Week 40, 4Q")]
    [InlineData("YY/M/D H:m:s.SSS", "26/10/3 14:5:9.045")]
    [InlineData("Z", "-07:00")]
    [InlineData("", "")]
    public void Formats_moment_patterns(string pattern, string expected)
    {
        Assert.Equal(expected, MomentFormatter.Format(Sample, pattern, En));
    }

    [Fact]
    public void Reads_old_clock_config()
    {
        var widget = new WidgetInstance
        {
            Type = "clock",
            Config = JsonDocument.Parse("""{"timePattern":"h:mm A","datePattern":"dddd"}""").RootElement.Clone(),
        };
        var config = widget.GetConfig<ClockConfig>();
        Assert.Equal("h:mm A", config.TimePattern);
        Assert.Equal("dddd", config.DatePattern);
    }

    [Fact]
    public void New_clock_has_old_defaults()
    {
        var widget = Services.WidgetTypeService.WidgetTypes["clock"].GetInstance();
        var config = widget.Config!.Value;
        Assert.Equal("HH:mm:ss", config.GetProperty("timePattern").GetString());
        Assert.Equal("YYYY-MM-DD", config.GetProperty("datePattern").GetString());
    }
}
