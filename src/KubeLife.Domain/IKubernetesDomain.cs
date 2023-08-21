using KubeLife.Core.Models;
using KubeLife.Domain.Models;
using KubeLife.Kubernetes.Models;

namespace KubeLife.Domain
{
    public interface IKubernetesDomain
    {
        Task<KubeLifeResult<List<KubeCronJobModelView>>> GetCronJobs();
        Task<KubeLifeResult<List<KubeRouteModel>>> GetAllRoutesForCluster();
        Task<KubeLifeResult<KubePodModel>> GetPodofJob(string kubeNamespace, string jobName);
        Task<KubeLogModel> GetLogOfPod(string kubeNamespace, string podName);
        Task<List<KubeLogModel>> GetLogOfAllPods(string kubeNamespace, List<string> podNames);
        Task<List<KubePodModel>> GetPodsOfRoute(string namepsaceParam, string routeName);
        Task<KubeLifeResult<KubeBuildModel>> TriggerBuild(string namespaceParameter, string buildConfigName);
    }
}