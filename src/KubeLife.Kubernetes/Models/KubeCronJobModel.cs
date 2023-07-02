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

        public bool IsJobDetailSet { get; internal set; } = false;
        public List<KubeJobModel> JobDetails { get; internal set; } = new List<KubeJobModel>();

        #region Calculated Properties
        private DateTime? _lastStartTime;
        public DateTime? LastStartTime
        {
            get
            {
                if (!IsJobDetailSet) return _lastStartTime;
                return JobDetails.Max(x => x.StartTime);
            }
            internal set => _lastStartTime = value;
        }

        private DateTime? _lastEndTime;
        public DateTime? LastEndTime
        {
            get
            {
                if (!IsJobDetailSet) return _lastEndTime;
                return JobDetails.Max(x => x.EndTime);
            }
            internal set => _lastEndTime = value;
        }

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

        #endregion
    }
}
