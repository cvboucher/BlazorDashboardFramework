using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BlazorDashboardFramework
{
    public class WidgetInstance
    {
        public event EventHandler? ConfigChanged;

        [JsonPropertyName("wid")]
        [JsonConverter(typeof(FlexibleIdConverter))]
        public string WidgetInstanceId { get; set; } = Guid.NewGuid().ToString();

        [JsonPropertyName("type")]
        public string Type
        {
            get => type;
            set => type = value ?? string.Empty;
        }
        private string type = string.Empty;

        [Required]
        [JsonPropertyName("title")]
        public string Title
        {
            get => title;
            set => title = value ?? string.Empty;
        }
        private string title = string.Empty;

        /// <summary>Raw widget configuration. Kept as JSON so settings unknown to the typed config survive.</summary>
        [JsonPropertyName("config")]
        [JsonConverter(typeof(WidgetConfigConverter))]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public JsonElement? Config { get; set; }

        [JsonPropertyName("titleTemplateUrl")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? TitleTemplateUrl { get; set; }

        [JsonPropertyName("editTemplateUrl")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? EditTemplateUrl { get; set; }

        /// <summary>Unknown properties (e.g. ADF's $$hashKey, styleClass, frameless) preserved on round-trip.</summary>
        [JsonExtensionData]
        public Dictionary<string, JsonElement>? ExtensionData { get; set; }

        public void OnConfigChanged()
        {
            ConfigChanged?.Invoke(this, new EventArgs());
        }

        /// <summary>Read the config as <typeparamref name="T"/>. Never throws; invalid values fall back to defaults.</summary>
        public T GetConfig<T>() where T : class, new()
        {
            return BdfJson.DeserializeConfig(Config, typeof(T)) as T ?? new T();
        }

        /// <summary>Read the config as <paramref name="type"/>. Never throws; invalid values fall back to defaults.</summary>
        public object? GetConfig(Type type)
        {
            if (type == null)
                return null;
            return BdfJson.DeserializeConfig(Config, type);
        }

        /// <summary>
        /// Store a typed config, merging it over the existing raw config so unknown keys are kept.
        /// </summary>
        public void SetConfig(object config)
        {
            ArgumentNullException.ThrowIfNull(config);
            Config = BdfJson.MergeConfig(Config, config);
            OnConfigChanged();
        }
    }
}
