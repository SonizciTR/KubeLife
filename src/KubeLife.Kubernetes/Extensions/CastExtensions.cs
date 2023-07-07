using k8s;
using k8s.Models;
using KubeLife.Kubernetes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using KubeLife.Core.Extensions;

namespace KubeLife.Kubernetes.Extensions
{
    internal static class CastExtensions
    {
        public static List<KubeCronJobModel> ToKubeCronJobModelList(this V1CronJobList source)
        {
            var target = new List<KubeCronJobModel>();

            foreach (var itmCron in source.Items)
            {
                var tmp = new KubeCronJobModel();
                tmp.Namespace = itmCron.Metadata.NamespaceProperty;
                tmp.CronJobName = itmCron.Metadata.Name;
                tmp.TimingRaw = itmCron.Spec.Schedule;
                tmp.IsSuspended = itmCron.Spec.Suspend ?? false;
                tmp.LastStartTime = itmCron.Status.LastScheduleTime;
                tmp.LastEndTime = itmCron.Status.LastSuccessfulTime;

                target.Add(tmp);
            }

            return target;
        }

        public static List<KubeJobModel> ToKubeJobModelList(this V1JobList source)
        {
            var target = new List<KubeJobModel>();

            foreach (var item in source.Items)
            {
                var tmp = new KubeJobModel();

                tmp.JobUniqueName = item.Metadata.Name;
                tmp.KubeNamespace = item.Metadata.NamespaceProperty;
                if (item.Metadata.OwnerReferences.Any())
                    tmp.OwnerCronJobName = item.Metadata.OwnerReferences[0].Name;
                tmp.StartTime = item.Status.StartTime;
                tmp.EndTime = item.Status.CompletionTime;
                tmp.IsSuccess = item.Status.Succeeded == 1;

                target.Add(tmp);
            }

            return target; //cronjob-retrain-score-28139130-5mpr6

        }

        public static V1CronJobList WhereLabelContains(this V1CronJobList source, string filterbyLabel)
        {
            var target = source.DeepCopyJson();
            target.Items = target.Items.Where(x => x.Labels()?
                                                    .Any(y => y.Key == filterbyLabel || y.Value == filterbyLabel)
                                                    ?? false)
                                        .ToList();
            return target;
        }

        public static List<KubePodModel> ToKubePodModelList(this V1PodList source)
        {
            var target = new List<KubePodModel>();

            foreach (var itm in source.Items)
            {
                var tmp = new KubePodModel();

                tmp.PodName = itm.Metadata.Name;
                tmp.Namespace = itm.Metadata.Namespace();
                tmp.CreateDate = itm.Metadata.CreationTimestamp;
                if (itm.Metadata.OwnerReferences.IsAny())
                    tmp.OwnerName = itm.Metadata.OwnerReferences[0].Name;

                target.Add(tmp);
            }

            return target;
        }
    }
}
