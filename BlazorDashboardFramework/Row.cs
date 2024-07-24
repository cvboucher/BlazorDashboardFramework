using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlazorDashboardFramework
{
    public class Row
    {
        [JsonPropertyName("columns")]
        public List<Column> Columns { get; set; } = new();
        public string? StyleClass { get; set; }
    }
}
