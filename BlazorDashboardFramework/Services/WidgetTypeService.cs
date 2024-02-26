using BlazorDashboardFramework.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorDashboardFramework.Services
{
    public class WidgetTypeService
    {
        public Dictionary<string, WidgetType> WidgetTypes { get; } = new();

        public WidgetTypeService()
        {
            // Add default widgets.
            var clockWidget = new WidgetType()
            {
                Type = "clock",
                Title = "Clock",
                Description = "Displays the current time.",
                DisplayComponent = typeof(ClockDisplayView),
                EditComponent = typeof(ClockEditView),
                ConfigType = typeof(ClockConfig),
                SetCollapsedEventCallback = true,
                SetHideHeaderEventCallback = true,
            };
            WidgetTypes.Add(clockWidget.Type, clockWidget);
        }

        public WidgetType? GetWidgetType(string type)
        {
            return WidgetTypes.Values.FirstOrDefault(x => type.Equals(x.Type, StringComparison.OrdinalIgnoreCase));
        }

        public object? GetWidgetInstanceConfig(string type, string? configJson)
        {
            var widgetType = GetWidgetType(type);
            if (widgetType == null)
                return null;
            return widgetType.GetConfig(configJson);
        }

    }
}
