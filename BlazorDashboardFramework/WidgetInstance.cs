using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorDashboardFramework
{
    public class WidgetInstance
    {
        public string WidgetInstanceId { get; set; } = Guid.NewGuid().ToString();
        required public string Type { get; set; }
        [Required]
        required public string Title { get; set; }
        public string? Config { get; set; }
    }
}
