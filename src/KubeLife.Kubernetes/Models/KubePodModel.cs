using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubeLife.Kubernetes.Models
{
    public class KubePodModel
    {
        public string PodName { get; internal set; }
        public string Namespace { get; set; }
        public string OwnerName { get; internal set; }
        public DateTime? CreateDate { get; internal set; }
    }
}
