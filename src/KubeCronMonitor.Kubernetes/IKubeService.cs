using KubeCronMonitor.Kubernetes.Models;

namespace KubeCronMonitor.Kubernetes
{
    public interface IKubeService
    {
        Task<List<KubeCronJobModel>> GetCronJobs();
    }
}