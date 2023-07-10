using KubeLife.Core.Models;
using KubeLife.Kubernetes.Models;

namespace KubeLife.Kubernetes.Services
{
    public interface IKubeRestService
    {
        Task<KubeLifeResult<KubeBuildModel>> TriggerBuildConfig(string namespaceParameter, string buildConfigName);
        Task<KubeLifeResult<List<KubeRouteModel>>> GetAllRoutesForCluster(int routeCount = 500, string filterbyLabel = null);
    }
}