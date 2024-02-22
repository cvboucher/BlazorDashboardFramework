using BlazorDashboardFramework.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorDashboardFramework.Services
{
    public class WidgetService
    {
        public Dictionary<string, Widget> Widgets { get; } = new();

        public WidgetService()
        {
            // Add default widgets.
            var clockWidget = new Widget()
            {
                Type = "clock",
                Title = "Clock",
                Description = "Displays the current time.",
                ContentComponent = typeof(ClockDisplay),
                EditComponent = typeof(ClockConfig)
            };
            Widgets.Add(clockWidget.Type, clockWidget);
        }

        public Widget? GetWidget(string type)
        {
            return Widgets.Values.FirstOrDefault(x => type.Equals(x.Type, StringComparison.OrdinalIgnoreCase));
        }

    }
}
