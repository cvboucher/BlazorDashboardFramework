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
                ContentComponent = typeof(ClockDisplayView),
                ConfigComponent = typeof(ClockConfigView),
                ConfigType = typeof(ClockConfig),
            };
            WidgetTypes.Add(clockWidget.Type, clockWidget);
        }

        public WidgetType? GetWidgetType(string type)
        {
            return WidgetTypes.Values.FirstOrDefault(x => type.Equals(x.Type, StringComparison.OrdinalIgnoreCase));
        }

    }
}
