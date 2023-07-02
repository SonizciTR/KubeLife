using System;
using System.Text.Json;

namespace KubeLife.Core.Extensions
{
    public static class JsonExtension
    {
        public static string ToJson<T>(this T source) where T : class => JsonSerializer.Serialize(source);

        public static T? ToModel<T>(this string jsonString) where T : class
        {
            if (string.IsNullOrWhiteSpace(jsonString)) return default;

            return JsonSerializer.Deserialize<T>(jsonString);
        }

        public static T DeepCopyJson<T>(this T model) where T : class => model.ToJson().ToModel<T>();
    }
}
