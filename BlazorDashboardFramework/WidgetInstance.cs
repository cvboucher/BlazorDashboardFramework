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
    }
}
