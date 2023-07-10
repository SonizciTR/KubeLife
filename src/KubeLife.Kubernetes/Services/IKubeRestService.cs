using KubeLife.Core.Models;

namespace KubeLife.Kubernetes.Services
{
    public interface IKubeRestService
    {
        Task<KubeLifeResult<KubeBuildModel>> TriggerBuildConfig(string namespaceParameter, string buildConfigName);
    }
}