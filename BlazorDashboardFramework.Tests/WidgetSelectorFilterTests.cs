using BlazorDashboardFramework.Services;

namespace BlazorDashboardFramework.Tests;

public class WidgetSelectorFilterTests
{
    [Fact]
    public void Without_a_filter_every_registered_type_is_selectable()
    {
        Assert.Null(WidgetTypeService.SelectorFilter);
        Assert.Contains(WidgetTypeService.SelectableWidgetTypes(), x => x.Type == "clock");
        Assert.Equal(WidgetTypeService.WidgetTypes.Count, WidgetTypeService.SelectableWidgetTypes().Count());
    }

    [Fact]
    public void The_filter_limits_the_dialog_but_not_lookups_of_existing_widgets()
    {
        try
        {
            WidgetTypeService.SelectorFilter = x => x.Type != "clock";
            Assert.DoesNotContain(WidgetTypeService.SelectableWidgetTypes(), x => x.Type == "clock");
            Assert.NotNull(new WidgetTypeService().GetWidgetType("clock"));
        }
        finally
        {
            WidgetTypeService.SelectorFilter = null;
        }
    }
}
