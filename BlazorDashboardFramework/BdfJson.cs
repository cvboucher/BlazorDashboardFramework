using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace BlazorDashboardFramework
{
    /// <summary>
    /// Central JSON settings for reading and writing dashboards.
    /// Reading is deliberately tolerant so that dashboards saved by the original
    /// AngularJS (angular-dashboard-framework) app load without errors or data loss.
    /// </summary>
    public static class BdfJson
    {
        /// <summary>Options used to read and write the dashboard model (rows/columns/widgets).</summary>
        public static JsonSerializerOptions DashboardOptions { get; } = new()
        {
            PropertyNameCaseInsensitive = true,
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
        };

        /// <summary>Options used to read typed widget configuration objects.</summary>
        public static JsonSerializerOptions ConfigReadOptions { get; } = CreateConfigReadOptions();

        /// <summary>Options used to write typed widget configuration objects (camelCase, like ADF).</summary>
        public static JsonSerializerOptions ConfigWriteOptions { get; } = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        private static JsonSerializerOptions CreateConfigReadOptions()
        {
            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                NumberHandling = JsonNumberHandling.AllowReadingFromString,
                AllowTrailingCommas = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
            };
            options.Converters.Add(new LenientValueConverterFactory());
            options.Converters.Add(new LenientStringConverter());
            return options;
        }

        /// <summary>Deserialize a dashboard model. Returns null for null/empty/invalid input.</summary>
        public static Dashboard? DeserializeDashboard(string? json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return null;
            return JsonSerializer.Deserialize<Dashboard>(json, DashboardOptions);
        }

        /// <summary>Deserialize a dashboard model.</summary>
        public static Dashboard? DeserializeDashboard(JsonElement json)
        {
            if (json.ValueKind != JsonValueKind.Object)
                return null;
            return json.Deserialize<Dashboard>(DashboardOptions);
        }

        public static string SerializeDashboard(Dashboard dashboard)
        {
            return JsonSerializer.Serialize(dashboard, DashboardOptions);
        }

        /// <summary>
        /// Deserialize a widget config. If the whole object can't be read (for example a value
        /// of an unexpected shape), the offending properties are skipped instead of failing.
        /// </summary>
        public static object? DeserializeConfig(JsonElement? config, Type type)
        {
            if (config == null || config.Value.ValueKind != JsonValueKind.Object)
                return Activator.CreateInstance(type);
            try
            {
                return config.Value.Deserialize(type, ConfigReadOptions) ?? Activator.CreateInstance(type);
            }
            catch (Exception ex) when (ex is JsonException or InvalidOperationException or NotSupportedException or FormatException or OverflowException)
            {
                Console.WriteLine($"BdfJson: config for {type.Name} could not be read as a whole, skipping invalid properties. {ex.Message}");
            }

            var cleaned = new JsonObject();
            foreach (var property in config.Value.EnumerateObject())
            {
                var single = new JsonObject { [property.Name] = JsonNode.Parse(property.Value.GetRawText()) };
                try
                {
                    single.Deserialize(type, ConfigReadOptions);
                    cleaned[property.Name] = JsonNode.Parse(property.Value.GetRawText());
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"BdfJson: ignoring config property '{property.Name}' for {type.Name}: {ex.Message}");
                }
            }
            try
            {
                return cleaned.Deserialize(type, ConfigReadOptions) ?? Activator.CreateInstance(type);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"BdfJson: using default config for {type.Name}: {ex.Message}");
                return Activator.CreateInstance(type);
            }
        }

        /// <summary>
        /// Merge a typed config object over the existing raw config. Keys that the typed object
        /// doesn't know about are preserved; keys it does know about are updated in place
        /// (keeping the original key casing). Null values are not added for keys that weren't present.
        /// </summary>
        public static JsonElement MergeConfig(JsonElement? existing, object config)
        {
            var merged = existing is { ValueKind: JsonValueKind.Object } e
                ? (JsonNode.Parse(e.GetRawText()) as JsonObject ?? new JsonObject())
                : new JsonObject();
            var updates = JsonSerializer.SerializeToNode(config, config.GetType(), ConfigWriteOptions) as JsonObject
                ?? new JsonObject();
            foreach (var (key, value) in updates)
            {
                var existingKey = merged.Select(x => x.Key)
                    .FirstOrDefault(x => string.Equals(x, key, StringComparison.OrdinalIgnoreCase));
                if (existingKey != null)
                    merged[existingKey] = value?.DeepClone();
                else if (value != null)
                    merged[key] = value.DeepClone();
            }
            return JsonSerializer.SerializeToElement(merged);
        }
    }
}
