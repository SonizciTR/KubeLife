using KubeLife.Kubernetes.Models.RestCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubeLife.Kubernetes.Models.Service
{
    public class KubeServiceSpec
    {
        public string host { get; set; }
        public KubeTo to { get; set; }
        public KubePort port { get; set; }
        public KubeTls tls { get; set; }
        public string wildcardPolicy { get; set; }
    }
}
