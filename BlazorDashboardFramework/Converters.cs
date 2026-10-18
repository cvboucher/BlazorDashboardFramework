using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BlazorDashboardFramework
{
    /// <summary>
    /// Reads an id that may have been stored as a string or a number (old ADF ids were numbers)
    /// and always writes it as a string.
    /// </summary>
    public sealed class FlexibleIdConverter : JsonConverter<string>
    {
        public override bool HandleNull => true;

        public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.String:
                    return reader.GetString();
                case JsonTokenType.Number:
                    using (var doc = JsonDocument.ParseValue(ref reader))
                        return doc.RootElement.GetRawText();
                case JsonTokenType.True:
                    return "true";
                case JsonTokenType.False:
                    return "false";
                case JsonTokenType.Null:
                    return Guid.NewGuid().ToString();
                default:
                    reader.Skip();
                    return Guid.NewGuid().ToString();
            }
        }

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value);
        }
    }

    /// <summary>
    /// Reads widget config as a raw <see cref="JsonElement"/> so unknown settings are preserved.
    /// ADF also accepted config stored as a JSON string; such strings are parsed into objects.
    /// </summary>
    public sealed class WidgetConfigConverter : JsonConverter<JsonElement?>
    {
        public override bool HandleNull => true;

        public override JsonElement? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
                return null;
            var element = JsonElement.ParseValue(ref reader);
            if (element.ValueKind == JsonValueKind.String)
            {
                var text = element.GetString();
                if (!string.IsNullOrWhiteSpace(text) && text.TrimStart().StartsWith('{'))
                {
                    try
                    {
                        using var doc = JsonDocument.Parse(text);
                        return doc.RootElement.Clone();
                    }
                    catch (JsonException) { }
                }
            }
            return element;
        }

        public override void Write(Utf8JsonWriter writer, JsonElement? value, JsonSerializerOptions options)
        {
            if (value == null || value.Value.ValueKind == JsonValueKind.Undefined)
                writer.WriteNullValue();
            else
                value.Value.WriteTo(writer);
        }
    }

    /// <summary>
    /// Tolerant reader for numeric and boolean config values: null becomes the default value,
    /// numbers may be strings ("5"), fractions are rounded for integer targets, and "" is null.
    /// AngularJS number inputs store null when cleared, which previously broke widget configs.
    /// </summary>
    public sealed class LenientValueConverterFactory : JsonConverterFactory
    {
        private static readonly HashSet<Type> Supported = new()
        {
            typeof(int), typeof(long), typeof(short), typeof(double), typeof(float), typeof(decimal), typeof(bool)
        };

        public override bool CanConvert(Type typeToConvert)
            => Supported.Contains(Nullable.GetUnderlyingType(typeToConvert) ?? typeToConvert);

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
            => (JsonConverter?)Activator.CreateInstance(typeof(LenientValueConverter<>).MakeGenericType(typeToConvert));
    }

    public sealed class LenientValueConverter<T> : JsonConverter<T>
    {
        private static readonly Type Underlying = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);

        public override bool HandleNull => true;

        public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            object? value = null;
            switch (reader.TokenType)
            {
                case JsonTokenType.Null:
                    break;
                case JsonTokenType.True:
                case JsonTokenType.False:
                    value = FromBool(reader.GetBoolean());
                    break;
                case JsonTokenType.Number:
                    value = Underlying == typeof(double) || Underlying == typeof(float)
                        ? FromDouble(reader.GetDouble())
                        : reader.TryGetDecimal(out var d) ? FromDecimal(d) : FromDouble(reader.GetDouble());
                    break;
                case JsonTokenType.String:
                    value = FromString(reader.GetString());
                    break;
                default:
                    reader.Skip();
                    break;
            }
            return value == null ? default : (T)value;
        }

        private static object FromBool(bool b)
            => Underlying == typeof(bool) ? b : FromDecimal(b ? 1 : 0);

        private static object? FromString(string? s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return null;
            s = s.Trim();
            if (Underlying == typeof(bool))
            {
                if (bool.TryParse(s, out var b)) return b;
                if (decimal.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out var n)) return n != 0;
                return null;
            }
            if (Underlying == typeof(double) || Underlying == typeof(float))
                return double.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out var dbl) ? FromDouble(dbl) : null;
            return decimal.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out var dec) ? FromDecimal(dec) : null;
        }

        private static object FromDouble(double value)
        {
            if (Underlying == typeof(double)) return value;
            if (Underlying == typeof(float)) return (float)value;
            if (Underlying == typeof(bool)) return value != 0;
            if (double.IsNaN(value) || double.IsInfinity(value)) value = 0;
            return FromDecimal((decimal)Math.Clamp(value, (double)decimal.MinValue, (double)decimal.MaxValue));
        }

        private static object FromDecimal(decimal value)
        {
            if (Underlying == typeof(decimal)) return value;
            if (Underlying == typeof(double)) return (double)value;
            if (Underlying == typeof(float)) return (float)value;
            if (Underlying == typeof(bool)) return value != 0;
            var rounded = Math.Round(value, MidpointRounding.AwayFromZero);
            if (Underlying == typeof(int)) return (int)Math.Clamp(rounded, int.MinValue, int.MaxValue);
            if (Underlying == typeof(long)) return (long)Math.Clamp(rounded, long.MinValue, long.MaxValue);
            if (Underlying == typeof(short)) return (short)Math.Clamp(rounded, short.MinValue, short.MaxValue);
            throw new NotSupportedException(Underlying.Name);
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            switch (value)
            {
                case null: writer.WriteNullValue(); break;
                case bool b: writer.WriteBooleanValue(b); break;
                case int i: writer.WriteNumberValue(i); break;
                case long l: writer.WriteNumberValue(l); break;
                case short s: writer.WriteNumberValue(s); break;
                case double d: writer.WriteNumberValue(d); break;
                case float f: writer.WriteNumberValue(f); break;
                case decimal m: writer.WriteNumberValue(m); break;
                default: throw new NotSupportedException(typeof(T).Name);
            }
        }
    }

    /// <summary>Tolerant string reader: numbers/booleans become their text; objects/arrays are ignored.</summary>
    public sealed class LenientStringConverter : JsonConverter<string>
    {
        public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.String:
                    return reader.GetString();
                case JsonTokenType.Null:
                    return null;
                case JsonTokenType.True:
                    return "true";
                case JsonTokenType.False:
                    return "false";
                case JsonTokenType.Number:
                    using (var doc = JsonDocument.ParseValue(ref reader))
                        return doc.RootElement.GetRawText();
                default:
                    reader.Skip();
                    return null;
            }
        }

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value);
        }
    }
}
