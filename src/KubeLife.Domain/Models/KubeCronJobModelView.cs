using KubeLife.Kubernetes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubeLife.Domain.Models
{
    public class KubeCronJobModelView : KubeCronJobModel
    {
        public DateTime NextRunTime { get; set; }

        public bool IsJobDetailSet { get; internal set; } = false;
        public List<KubeJobModel> JobDetails { get; set; } = new List<KubeJobModel>();

        public KubeBuildModel LastBuild { get; internal set; }

        #region Calculated Properties
        public DateTime? LastStartTimeLatest
        {
            get
            {
                if (!IsJobDetailSet) return LastStartTime;
                return JobDetails.Max(x => x.StartTime);
            }
        }

        public DateTime? LastEndTimeLatest
        {
            get
            {
                if (!IsJobDetailSet) return LastEndTime;
                return JobDetails.Max(x => x.EndTime);
            }
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
