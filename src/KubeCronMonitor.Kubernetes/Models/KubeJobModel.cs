using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubeCronMonitor.Kubernetes.Models
{
    public class KubeJobModel
    {
        public string JobUniqueName { get; internal set; }
        public string KubeNamespace { get; internal set; }
        public DateTime? StartTime { get; internal set; }
        public DateTime? EndTime { get; internal set; }
        public bool IsSuccess { get; internal set; }
        public string? OwnerCronJobName { get; internal set; }
    }
}
