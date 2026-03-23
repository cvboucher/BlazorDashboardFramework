using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlazorDashboardFramework
{
    public class WidgetInstance
    {
        public event EventHandler? ConfigChanged;

        [JsonPropertyName("wid")]
        public string WidgetInstanceId { get; set; } = Guid.NewGuid().ToString();
        [JsonPropertyName("type")]
        required public string Type { get; set; }
        [Required]
        [JsonPropertyName("title")]
        required public string Title { get; set; }
        [JsonPropertyName("config")]
        [JsonConverter(typeof(InfoToJsonElementConverter))]
        public JsonElement? Config { get; set; }
        //[JsonPropertyName("config")]
        //[JsonConverter(typeof(InfoToStringConverter))]
        //public string? Config { get; set; }
        [JsonPropertyName("titleTemplateUrl")]
        public string? TitleTemplateUrl { get; set; }
        [JsonPropertyName("editTemplateUrl")]
        public string? EditTemplateUrl { get; set; }

        public void OnConfigChanged()
        {
            ConfigChanged?.Invoke(this, new EventArgs());
        }

        public T GetConfig<T>() where T : class
        {
            try
            {
                if (Config == null)
                    return Activator.CreateInstance<T>();
                else
                    return JsonSerializer.Deserialize<T>(Config.Value, new JsonSerializerOptions()
                        {
                            PropertyNameCaseInsensitive = true,
                        })
                    ?? Activator.CreateInstance<T>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Activator.CreateInstance<dynamic>();
            }
        }

        //public T GetConfig<T>(T type) where T : class
        //{
        //    try
        //    {
        //        if (type == null || Config == null)
        //            return Activator.CreateInstance<T>();
        //        else
        //            return JsonSerializer.Deserialize(Config.Value, type, new JsonSerializerOptions()
        //            {
        //                PropertyNameCaseInsensitive = true,
        //            })
        //                ?? Activator.CreateInstance<T>();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.ToString());
        //        return Activator.CreateInstance<T>();
        //    }
        //}

        public object? GetConfig(Type type)
        {
            if (type == null || Config == null)
                return null;
            try
            {
                if (this.Config == null)
                    return Activator.CreateInstance(type);
                else
                    return JsonSerializer.Deserialize(Config.Value, type, new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true,
                    })
                        ?? Activator.CreateInstance(type);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Activator.CreateInstance(type);
            }
        }
    }
}
