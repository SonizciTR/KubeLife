using AutoMapper;
using k8s.KubeConfigModels;
using KubeLife.Core.Models;
using KubeLife.Domain.Models;
using KubeLife.Kubernetes;
using KubeLife.Kubernetes.Models;
using NCrontab;
using KubeLife.Core.Extensions;
using System.Collections.Generic;

namespace KubeLife.Domain
{
    public class KubernetesDomain : IKubernetesDomain
    {
        public KubernetesDomain(IKubeService kubeService, IMapper mapper)
        {
            this.kubeService = kubeService;
            this.mapper = mapper;
        }

        private readonly IKubeService kubeService;
        private readonly IMapper mapper;

        public async Task<List<KubeCronJobModelView>> GetCronJobs(string filterbyLabel)
        {
            var crnJobsSource = await kubeService.GetCronJobs(filterbyLabel);
            var target = mapper.Map<List<KubeCronJobModelView>>(crnJobsSource);

            var jobDetails = new Dictionary<string, List<KubeJobModel>>();
            foreach (var itm in target)
            {
                string tmpKey = itm.Namespace;
                List<KubeJobModel> tmpDetail;
                if (jobDetails.ContainsKey(tmpKey))
                    tmpDetail = jobDetails[tmpKey];
                else
                {
                    tmpDetail = await kubeService.GetJobsbyNamespace(tmpKey);
                    jobDetails.Add(tmpKey, tmpDetail);
                }   

                itm.IsJobDetailSet = true;
                itm.JobDetails = tmpDetail;
            }

            target = AddingNextRunTime(target);

            return target;
        }

        internal List<KubeCronJobModelView> AddingNextRunTime(List<KubeCronJobModelView> data)
        {
            foreach (var itm in data)
            {
                var schedule = CrontabSchedule.Parse(itm.TimingRaw);
                itm.NextRunTime = schedule.GetNextOccurrence(DateTime.Now);
            }

            return data;
        }

        public async Task<KubeLifeResult<string>> GetLogofJob(string kubeNamespace, string jobName)
        {
            var allPods = await kubeService.GetPodsofNamespace(kubeNamespace);
            var foundPod = allPods.FirstOrDefault(x => x.OwnerName == jobName);
            if (foundPod == null) return new KubeLifeResult<string>(false, $"Pod could not found on cluster");

            var log = await kubeService.GetLogofPod(foundPod.Namespace, foundPod.PodName);
            if (log == null) return new KubeLifeResult<string>(false, "No log found of Pod");

            return new KubeLifeResult<string>(log);
        }
    }
}