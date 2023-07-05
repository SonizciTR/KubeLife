using System;
using System.Collections.Generic;
using System.Text;

namespace KubeLife.Core.Extensions
{
    public static class StringHelper
    {
        public static string ToHtml(this string source)
        {
            return source.Replace("\n", "<br/>");
        }
    }
}
