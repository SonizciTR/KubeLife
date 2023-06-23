using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubeCronMonitor.Kubernetes.Models
{
    public class KubeConfigModel
    {
        public string ServerUrl { get; set; }
        public string AccessToken { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
    }
}
