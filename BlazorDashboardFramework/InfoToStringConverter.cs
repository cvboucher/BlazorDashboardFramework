//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Text.Json;
//using System.Text.Json.Serialization;
//using System.Threading.Tasks;

//namespace BlazorDashboardFramework
//{
//    public class InfoToStringConverter : JsonConverter<string>
//    {
//        public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
//        {
//            using (var jsonDoc = JsonDocument.ParseValue(ref reader))
//            {
//                //var value = jsonDoc.RootElement.GetRawText();
//                try
//                {
//                    return jsonDoc.RootElement.GetString();
//                }
//                catch (Exception)
//                {
//                    return jsonDoc.RootElement.GetRawText();
//                }
//                //var value = jsonDoc.RootElement.GetString();
//                //return value;
//            }
//        }

//        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
//        {
//            writer.WriteStringValue(value);
//        }
//    }
//}
