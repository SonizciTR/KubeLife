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
            switch(propertyType.Name)
            {
                case "String":
                    return Guid.NewGuid().ToString();
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
    }
}