using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace KubeLife.Core.Extensions
{
    public static class JsonExtension
    {
        public static string ToJson<T>(this T source) where T : class => JsonSerializer.Serialize(source);

        public static T? ToModel<T>(this string jsonString) where T : class
        {
            if (string.IsNullOrWhiteSpace(jsonString)) return default;

            return JsonSerializer.Deserialize<T>(jsonString);
        }//var dynamicObject = JsonConvert.DeserializeObject<dynamic>(jsonString)!;

        public static string GetNodeValueAsString(this string jsonString, params string[] keys)
        {
            if (string.IsNullOrWhiteSpace(jsonString)) return default;

            var node = JsonSerializer.Deserialize<JsonNode>(jsonString)!;

            foreach ( var key in keys ) 
            {
                node = node[key];
            }

            return node.GetValue<string>();
        }

        public static T DeepCopyJson<T>(this T model) where T : class => model.ToJson().ToModel<T>();
    }
}
