using BlazorDashboardFramework.Services;
using Blazored.Modal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorDashboardFramework
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddBlazorDashboardFramework(this IServiceCollection services)
        {

            services.AddBlazoredModal();

            services.AddScoped<EditModeService>();
            services.AddSingleton<LayoutService>();
            services.AddSingleton<WidgetService>();

            return services;
        }
    }
}
