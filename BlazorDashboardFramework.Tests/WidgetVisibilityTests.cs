namespace BlazorDashboardFramework.Tests;

public class WidgetVisibilityTests
{
    [Fact]
    public void Visible_by_default()
    {
        var visibility = new WidgetVisibility();
        Assert.False(visibility.HiddenByWidget);
        Assert.False(visibility.IsHidden(editMode: false));
        Assert.False(visibility.IsHidden(editMode: true));
    }

    [Fact]
    public void SetHidden_hides_outside_edit_mode_only()
    {
        var visibility = new WidgetVisibility();
        visibility.SetHidden(true);
        Assert.True(visibility.IsHidden(editMode: false));
        Assert.False(visibility.IsHidden(editMode: true));
    }

    [Fact]
    public void Changed_fires_only_when_the_value_changes()
    {
        var visibility = new WidgetVisibility();
        var count = 0;
        visibility.Changed += () => count++;
        visibility.SetHidden(false);
        visibility.SetHidden(true);
        visibility.SetHidden(true);
        visibility.SetHidden(false);
        Assert.Equal(2, count);
        Assert.False(visibility.HiddenByWidget);
    }
}
