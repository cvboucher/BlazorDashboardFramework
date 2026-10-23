using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BlazorDashboardFramework
{
    public class Column
    {
        private List<WidgetInstance> widgets = new();
        private List<Row> rows = new();

        [JsonPropertyName("cid")]
        [JsonConverter(typeof(FlexibleIdConverter))]
        public string ColumnId { get; set; } = Guid.NewGuid().ToString();

        [JsonPropertyName("styleClass")]
        public string StyleClass
        {
            get => styleClass;
            set => styleClass = value ?? string.Empty;
        }
        private string styleClass = string.Empty;

        [JsonIgnore]
        public List<WidgetInstance> Widgets
        {
            get => widgets;
            set => widgets = value ?? new();
        }

        [JsonIgnore]
        public List<Row> Rows
        {
            get => rows;
            set => rows = value ?? new();
        }

        /// <summary>
        /// JSON surrogate for <see cref="Widgets"/>. Written for leaf columns (as ADF does) and
        /// for any column that actually has widgets; omitted for container columns.
        /// </summary>
        [JsonPropertyName("widgets")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public List<WidgetInstance>? WidgetsJson
        {
            get => widgets.Count > 0 || rows.Count == 0 ? widgets : null;
            set => Widgets = value!;
        }

        /// <summary>JSON surrogate for <see cref="Rows"/>. Only written for container columns, like ADF.</summary>
        [JsonPropertyName("rows")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public List<Row>? RowsJson
        {
            get => rows.Count > 0 ? rows : null;
            set => Rows = value!;
        }

        /// <summary>Unknown properties (e.g. ADF's $$hashKey) preserved on round-trip.</summary>
        [JsonExtensionData]
        public Dictionary<string, JsonElement>? ExtensionData { get; set; }

        [JsonIgnore]
        public EventHandler<List<WidgetInstance>>? WidgetsChanged { get; set; }

        public void OnWidgetsChanged()
        {
            WidgetsChanged?.Invoke(this, this.Widgets);
        }
    }
}
