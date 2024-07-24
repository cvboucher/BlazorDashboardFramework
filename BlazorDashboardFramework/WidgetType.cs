using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorDashboardFramework
{
    public class WidgetType
    {
        required public string Type { get; set; }
        required public string Title { get; set; }
        public string? Description { get; set; }
        required public Type DisplayComponent { get; set; }
        public Type? EditComponent { get; set; }
        public Type? ConfigType { get; set; }
        public JsonElement? Config { get; set; }
        public bool HideWidget { get; set; }
        public bool HideHeader { get; set; }
        public bool Collapsed { get; set; }
        

        public WidgetInstance GetInstance()
        {
            return new WidgetInstance()
            {
                Type = this.Type,
                Title = this.Title,
                Config = this.Config
            };
        }

        public object? GetConfig(JsonElement? jsonElement)
        {
            if (ConfigType == null)
                return null;
            try
            {
                if (jsonElement == null)
                    return Activator.CreateInstance(ConfigType);
                else
                    return JsonSerializer.Deserialize(jsonElement.Value, ConfigType, new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true,
                    }) 
                        ?? Activator.CreateInstance(ConfigType);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Activator.CreateInstance(ConfigType);
            }
        }

        public object? GetConfig(JsonDocument? jsonDocument)
        {
            if (ConfigType == null)
                return null;
            try
            {
                if (jsonDocument == null)
                    return Activator.CreateInstance(ConfigType);
                else
                    return jsonDocument.Deserialize(ConfigType, new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true,
                    }) // .DeserializeFromCamelCase(ConfigType) //  JsonSerializer.Deserialize(json.Replace("u022", "\""), ConfigType)
                        ?? Activator.CreateInstance(ConfigType);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Activator.CreateInstance(ConfigType);
            }
        }
        public object? GetConfig(string? json)
        {
            if (ConfigType == null)
                return null;
            try
            {

                if (string.IsNullOrWhiteSpace(json))
                    return Activator.CreateInstance(ConfigType);
                else
                    return json.DeserializeFromCamelCase(ConfigType) //  JsonSerializer.Deserialize(json.Replace("u022", "\""), ConfigType)
                        ?? Activator.CreateInstance(ConfigType);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Activator.CreateInstance(ConfigType);
            }
        }
    }
}
