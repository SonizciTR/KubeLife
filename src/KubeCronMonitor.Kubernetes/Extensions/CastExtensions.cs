using k8s.Models;
using KubeCronMonitor.Kubernetes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubeCronMonitor.Kubernetes.Extensions
{
    public static class CastExtensions
    {
        public static List<KubeCronJobModel> ToKubeCronJobModelList(this V1CronJobList source)
        {
            var target = new List<KubeCronJobModel>();

            foreach (var item in source.Items)
            {
                var tmp = new KubeCronJobModel();
                tmp.Namespace = item.Metadata.NamespaceProperty;
                tmp.CronJobName = item.Metadata.Name;
                tmp.TimingRaw = item.Spec.Schedule;
                tmp.IsSuspended = item.Spec.Suspend ?? false;
                tmp.LastStartTime = item.Status.LastScheduleTime;
                tmp.LastEndTime = item.Status.LastSuccessfulTime;

                target.Add(tmp);
            }

            return target;
        }
    }
}
