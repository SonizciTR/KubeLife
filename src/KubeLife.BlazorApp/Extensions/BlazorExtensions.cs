using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;

namespace KubeLife.BlazorApp.Extensions
{
    public static class BlazorExtensions
    {
        public static Dictionary<string, string> QueryStringToDictionary(this NavigationManager source)
        {
            var target = new Dictionary<string, string>();

            var uri = source.ToAbsoluteUri(source.Uri);
            var queryStrings = QueryHelpers.ParseQuery(uri.Query);
            foreach (var queryString in queryStrings)
            {
                target.Add(queryString.Key, queryString.Value.ToString());
            }

            return target;
        }

        public static string QueryStringSingle(this NavigationManager source, string name)
        {
            var target = source.QueryStringToDictionary();
            var frst = target.FirstOrDefault(x => x.Key == name);

            return frst.Value;
        }
    }
}
