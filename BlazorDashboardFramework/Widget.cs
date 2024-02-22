using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorDashboardFramework
{
    public class Widget
    {
        required public string Type { get; set; }
        required public string Title { get; set; }
        public string? Description { get; set; }
        public string? Config { get; set; }
        public bool HideWidget { get; set; }
        public bool HideHeader { get; set; }
        public bool Collapsed { get; set; }
        required public Type ContentComponent { get; set; }
        public Type? EditComponent { get; set; }

        public WidgetInstance GetInstance()
        {
            return new WidgetInstance()
            {
                Type = this.Type,
                Title = this.Title,
                Config = this.Config,
                HideWidget = this.HideWidget,
                HideHeader = this.HideHeader,
                Collapsed = this.Collapsed
            };
        }
    }
}
