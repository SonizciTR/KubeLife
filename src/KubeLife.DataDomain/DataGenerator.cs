using System;
using System.Reflection;

namespace KubeLife.DataDomain
{
    public static class DataGenerator
    {
        public static T GenerateData<T>() where T : class, new()
        {
            var target = new T();

            var allProps = target.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in allProps)
            {
                if (null != prop && prop.CanWrite)
                {
                    var tmpVal = GetRandomValue(prop.PropertyType);
                    prop.SetValue(target, tmpVal, null);
                }
            }

            return target;
        }

        private static object GetRandomValue(Type propertyType)
        {
            Random r = new Random();
            switch (propertyType.Name)
            {
                case "String":
                    var tmp = Guid.NewGuid().ToString();
                    return new string(Enumerable.Repeat(tmp, r.Next(50, 255))
                                .Select(s => s[r.Next(s.Length)]).ToArray());
                case "Int32":
                    return r.Next(0, 255);
                case "Double":
                    return r.NextDouble() * r.Next(0, 255);
                case "DateTime":
                    return DateTime.Now.AddDays(r.Next(0, 255)).AddMinutes(r.Next(0, 2505));
                case "Boolean":
                    return r.Next(0, 255) % 1 == 0;
            }

            return default;
        }

        public static List<T> GenerateData<T>(int count) where T : class, new()
        {
            var target = new List<T>();
            
            for (int i = 0; i < count; i++)
            {
                var tmp = GenerateData<T>();
                target.Add(tmp);
            }

            return target;
        }
    }
}