using KubeLife.Core.Models;
using KubeLife.Kubernetes.Models;

namespace KubeLife.Domain
{
    public interface IKubernetesDomain
    {
        Task<List<KubeCronJobModel>> GetCronJobs(string filterbyLabel);
        Task<KubeLifeResult<string>> GetLogofJob(string kubeNamespace, string jobName);
    }
}