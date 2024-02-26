using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorDashboardFramework
{
    public class WidgetType
    {
        required public string Type { get; set; }
        required public string Title { get; set; }
        public string? Description { get; set; }
        required public Type DisplayComponent { get; set; }
        public Type? EditComponent { get; set; }
        public Type? ConfigType { get; set; }
        public string? Config { get; set; }
        public bool HideWidget { get; set; }
        public bool SetHideWidgetEventCallback { get; set; }
        public bool HideHeader { get; set; }
        public bool SetHideHeaderEventCallback { get; set; }
        public bool ToggleHideHeaderEventCallback { get; set; }
        public bool Collapsed { get; set; }
        public bool SetCollapsedEventCallback { get; set; }
        public bool ToggleCollapsedEventCallback { get; set; }
        

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

        public object? GetConfig(string? json)
        {
            if (ConfigType == null)
                return null;
            if (string.IsNullOrWhiteSpace(json))
                return Activator.CreateInstance(ConfigType);
            else
                return JsonSerializer.Deserialize(json, ConfigType)
                    ?? Activator.CreateInstance(ConfigType);
        }
    }
}
