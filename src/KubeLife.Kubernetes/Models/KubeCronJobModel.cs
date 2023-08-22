using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubeLife.Kubernetes.Models
{
    public class KubeCronJobModel
    {
        public string Namespace { get; set; }
        public string CronJobName { get; set; }
        public string TimingRaw { get; set; }
        public bool IsSuspended { get; set; }
        public DateTime? LastStartTime { get; set; }
        public DateTime? LastEndTime { get; set; }
    }
}
