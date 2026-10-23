using System.Text.Json;
using System.Text.Json.Serialization;

namespace BlazorDashboardFramework
{
    public class Row
    {
        private List<Column> columns = new();

        [JsonPropertyName("columns")]
        public List<Column> Columns
        {
            get => columns;
            set => columns = value ?? new();
        }

        [JsonPropertyName("styleClass")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? StyleClass { get; set; }

        /// <summary>Unknown properties (e.g. ADF's $$hashKey) preserved on round-trip.</summary>
        [JsonExtensionData]
        public Dictionary<string, JsonElement>? ExtensionData { get; set; }
    }
}
