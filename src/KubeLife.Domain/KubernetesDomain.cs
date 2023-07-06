using AutoMapper;
using k8s.KubeConfigModels;
using KubeLife.Core.Models;
using KubeLife.Domain.Models;
using KubeLife.Kubernetes;
using KubeLife.Kubernetes.Models;
using NCrontab;
using KubeLife.Core.Extensions;

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
            var data = await kubeService.GetCronJobs(filterbyLabel, true);
            var target = new List<KubeCronJobModelView>();

            foreach (var itm in data)
            {
                var tmp = mapper.Map<KubeCronJobModelView>(itm);
                var schedule = CrontabSchedule.Parse(tmp.TimingRaw);
                tmp.NextRunTime = schedule.GetNextOccurrence(DateTime.Now);
                target.Add(tmp);
            }

            return target;
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