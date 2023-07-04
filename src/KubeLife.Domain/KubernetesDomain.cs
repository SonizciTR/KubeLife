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
    }
}