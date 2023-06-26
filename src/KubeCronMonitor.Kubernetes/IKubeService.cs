using KubeCronMonitor.Kubernetes.Models;

namespace KubeCronMonitor.Kubernetes
{
    public interface IKubeService
    {
        Task<List<KubeCronJobModel>> GetCronJobs(string filterbyLabel = null, bool includeJobDetails = true);
        Task<List<KubeJobModel>> GetJobsbyNamespace(string kubeNamespace);
    }
}