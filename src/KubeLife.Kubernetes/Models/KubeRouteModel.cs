using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubeLife.Kubernetes.Models
{
    public class KubeRouteModel
    {
        public string Name { get; set; }
        public string Namespace { get; set; }
        public string Host { get; internal set; }
        public string HostLink => $"https://{Host}";
    }

}
