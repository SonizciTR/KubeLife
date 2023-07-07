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
        public bool IsSuspended { get; internal set; }
        public DateTime? LastStartTime { get; internal set; }
        public DateTime? LastEndTime { get; internal set; }
    }
}
