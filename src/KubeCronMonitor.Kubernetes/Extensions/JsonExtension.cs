using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace KubeCronMonitor.Kubernetes.Extensions
{
    internal static class JsonExtension
    {
        public static string ToJson<T>(this T source) where T : class => JsonSerializer.Serialize(source);

        public static T ToModel<T>(this string jsonString) where T : class => JsonSerializer.Deserialize<T>(jsonString);

        public static T DeepCopy<T>(this T model) where T: class => model.ToJson().ToModel<T>();
    }
}
