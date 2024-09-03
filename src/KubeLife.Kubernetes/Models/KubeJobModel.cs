using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubeLife.Kubernetes.Models
{
    public class KubeJobModel
    {
        public string JobUniqueName { get; set; }
        public string KubeNamespace { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool IsSuccess { get; set; }
        public string? OwnerCronJobName { get; set; }
        public bool IsStillRunning { get; internal set; }

        public double? Duration
        {
            get
            {
                if (EndTime == null || StartTime == null)
                    return 0;

                TimeSpan diff = (TimeSpan)(EndTime - StartTime);
                return diff.TotalMinutes;
            }
        }
    }
}
