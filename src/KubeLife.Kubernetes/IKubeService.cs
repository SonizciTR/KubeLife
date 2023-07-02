using KubeLife.Kubernetes.Models;

namespace KubeLife.Kubernetes
{
    public interface IKubeService
    {
        Task<List<KubeCronJobModel>> GetCronJobs(string filterbyLabel = null, bool includeJobDetails = true);
        Task<List<KubeJobModel>> GetJobsbyNamespace(string kubeNamespace);
    }
}