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

        public static JsonSerializerOptions DefaultJsonSerializerOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        public static T? DeepCopy<T>(this T self)
        {
            var serialized = JsonSerializer.Serialize(self);
            return JsonSerializer.Deserialize<T>(serialized);
        }

        public static string SerializeWithCamelCase<T>(this T data)
        {
            return JsonSerializer.Serialize(data, DefaultJsonSerializerOptions);
        }

        public static T DeserializeFromCamelCase<T>(this string json)
        {
            return JsonSerializer.Deserialize<T>(json, DefaultJsonSerializerOptions);
        }

        public static object DeserializeFromCamelCase(this string json, Type type)
        {
            return JsonSerializer.Deserialize(json, type, DefaultJsonSerializerOptions);
        }

        public static IServiceCollection AddBlazorDashboardFramework(this IServiceCollection services)
        {
            services.AddSingleton<Widgets.Clock.ClockWidgetService>();
            services.AddScoped<EditModeService>();
            services.AddSingleton<StructureService>();
            services.AddSingleton<SortableListService>();
            services.AddSingleton<WidgetTypeService>();

            return services;
        }
    }
}
