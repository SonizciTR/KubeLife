using KubeLife.Kubernetes;
using KubeLife.Kubernetes.Models;

namespace KubeLife.Domain
{
    public class KubernetesDomain : IKubernetesDomain
    {
        public KubernetesDomain(IKubeService kubeService)
        {
            KubeService = kubeService;
        }

        private readonly IKubeService KubeService;

        public async Task<List<KubeCronJobModel>> GetCronJobs(string filterbyLabel)
        {
            return await KubeService.GetCronJobs(filterbyLabel, true);
        }
    }
}