using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlazorDashboardFramework
{
    public class Column
    {
        [JsonPropertyName("cid")]
        public string ColumnId { get; set; } = Guid.NewGuid().ToString();
        [JsonPropertyName("styleClass")]
        required public string StyleClass { get; set; }
        [JsonPropertyName("widgets")]
        public List<WidgetInstance> Widgets { get; set; } = new();
        [JsonPropertyName("rows")]
        public List<Row> Rows { get; set; } = new();

        [JsonIgnore]
        public EventHandler<List<WidgetInstance>>? WidgetsChanged { get; set; }

        public void OnWidgetsChanged()
        {
            WidgetsChanged?.Invoke(this, this.Widgets);
        }
    }
}
