using BlazorDashboardFramework.Services;
using BlazorDashboardFramework.Widgets;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlazorDashboardFramework
{
    public static class ExtensionMethods
    {
        public static T? DeepCopy<T>(this T self)
        {
            var serialized = JsonSerializer.Serialize(self);
            return JsonSerializer.Deserialize<T>(serialized);
        }

        public static IServiceCollection AddBlazorDashboardFramework(this IServiceCollection services)
        {
            services.AddSingleton<Widgets.Clock.ClockWidgetService>();
            services.AddScoped<EditModeService>();
            services.AddSingleton<LayoutService>();
            services.AddSingleton<WidgetTypeService>();

            return services;
        }
    }
}
