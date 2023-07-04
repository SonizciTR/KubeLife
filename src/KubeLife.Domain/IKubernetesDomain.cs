using KubeLife.Kubernetes.Models;

namespace KubeLife.Domain
{
    public interface IKubernetesDomain
    {
        Task<List<KubeCronJobModel>> GetCronJobs(string filterbyLabel);
    }
}