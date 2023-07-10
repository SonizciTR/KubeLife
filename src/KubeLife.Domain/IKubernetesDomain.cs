using KubeLife.Core.Models;
using KubeLife.Domain.Models;
using KubeLife.Kubernetes.Models;

namespace KubeLife.Domain
{
    public interface IKubernetesDomain
    {
        Task<KubeLifeResult<List<KubeRouteModel>>> GetAllRoutesForCluster();
        Task<List<KubeCronJobModelView>> GetCronJobs();
        Task<KubeLifeResult<string>> GetLogofJob(string kubeNamespace, string jobName);
        Task<KubeLifeResult<KubeBuildModel>> TriggerBuild(string namespaceParameter, string buildConfigName);
    }
}