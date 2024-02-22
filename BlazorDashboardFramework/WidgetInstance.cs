using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorDashboardFramework
{
    public class WidgetInstance
    {
        public string WidgetInstanceId { get; set; } = Guid.NewGuid().ToString();
        required public string Type { get; set; }
        required public string Title { get; set; }
        public string? Config { get; set; }
        public bool HideWidget { get; set; }
        public bool HideHeader { get; set; }
        public bool Collapsed { get; set; }
    }
}
