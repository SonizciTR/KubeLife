using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubeCronMonitor.Kubernetes.Models
{
    public class KubeCronJobModel
    {
        public string Namespace { get; set; }
        public string CronJobName { get; set; }
        public string TimingRaw { get; set; }
        public bool IsSuspended { get; internal set; }
        public DateTime? LastStartTime { get; internal set; }
        public DateTime? LastEndTime { get; internal set; }
        public bool IsJobDetailSet { get; internal set; } = false;
        public List<KubeJobModel> JobDetails { get; internal set; } = new List<KubeJobModel>();

        //Calculated Properties
        public double AvgRunTimeMinutes
        {
            get
            {
                if (!IsJobDetailSet) return -1;
                double cnt = 0, total = 0;
                foreach (var itm in JobDetails)
                {
                    if (itm.StartTime == null || itm.EndTime == null) continue;
                    cnt++;
                    total += (itm.EndTime - itm.StartTime).Value.TotalMinutes;
                }

                return Math.Round(total / cnt, 2);
            }
        }
        public int SuccessCount
        {
            get
            {
                if (!IsJobDetailSet) return -1;
                return JobDetails.Count(x => x.IsSuccess);
            }
        }
        public int FailureCount
        {
            get
            {
                if (!IsJobDetailSet) return -1;
                return JobDetails.Count(x => !x.IsSuccess);
            }
        }
    }
}
