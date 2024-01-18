using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubeLife.Kubernetes.Models
{
    public class KubeConfigModel
    {
        public string KubeServerUrl { get; set; }
        public string KubeAccessToken { get; set; }
        public string KubeUserName { get; set; }
        public string KubePassWord { get; set; }
        public string? S3ModelAccessKey { get; set; }
        public string? S3ModelEndpoint { get; set; }
        public string? S3ModelSecretKey { get; set; }
    }
}
