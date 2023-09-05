using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubeLife.Kubernetes.Models.RestCommon
{
    public class RawKubeResponseStatus
    {
        public string kind { get; set; }
        public string apiVersion { get; set; }
        public string status { get; set; }
        public string message { get; set; }
        public string reason { get; set; }
        public RawKubeResponseDetails details { get; set; }
        public int code { get; set; }
    }

    public class RawKubeResponseDetails
    {
        public string name { get; set; }
        public string group { get; set; }
        public string kind { get; set; }
    }

}
