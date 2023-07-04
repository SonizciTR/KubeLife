using KubeLife.Core.Models;
using KubeLife.Kubernetes;
using KubeLife.Kubernetes.Models;

namespace KubeLife.Domain
{
    public class KubernetesDomain : IKubernetesDomain
    {
        public KubernetesDomain(IKubeService kubeService)
        {
            this.kubeService = kubeService;
        }

        private readonly IKubeService kubeService;

        public async Task<List<KubeCronJobModel>> GetCronJobs(string filterbyLabel)
        {
            var data = await kubeService.GetCronJobs(filterbyLabel, true);

            return data;
        }

        public async Task<KubeLifeResult<string>> GetLogofJob(string kubeNamespace, string jobName)
        {
            var allPods = await kubeService.GetPodsofNamespace(kubeNamespace);
            var foundPod = allPods.FirstOrDefault(x => x.OwnerName == jobName);
            if (foundPod == null) return new KubeLifeResult<string>(false, $"Pod could not found on cluster");

            var log = await kubeService.GetLogofPod(foundPod.PodName, foundPod.Namespace);
            if (log == null) return new KubeLifeResult<string>(false, "No log found of Pod");

            return new KubeLifeResult<string>(log);
        }
    }
}