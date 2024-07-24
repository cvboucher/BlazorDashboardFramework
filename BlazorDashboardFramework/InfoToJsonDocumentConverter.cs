//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Text.Json;
//using System.Text.Json.Serialization;
//using System.Threading.Tasks;

//namespace BlazorDashboardFramework
//{
//    internal class InfoToJsonDocumentConverter : JsonConverter<JsonDocument>
//    {
//        public override JsonDocument? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
//        {
//            using (var jsonDoc = JsonDocument.ParseValue(ref reader))
//            {
//                JsonDocument? returnVal;
//                try
//                {
//                    returnVal = JsonDocument.Parse(jsonDoc.RootElement.GetString());
//                }
//                catch (Exception)
//                {
//                    returnVal = JsonDocument.Parse(jsonDoc.RootElement.GetRawText());
//                }
//                return returnVal;
//            }
//        }

//        public override void Write(Utf8JsonWriter writer, JsonDocument value, JsonSerializerOptions options)
//        {
//            writer.WriteStringValue(value.SerializeWithCamelCase());
//        }
//    }
//}
