using KubeLife.Core.Models;

namespace KubeLife.Kubernetes.Services
{
    internal interface IKubeRestService
    {
        Task<KubeLifeResult<KubeBuildModel>> TriggerBuildConfig(string namespaceParameter, string buildConfigName);
    }
}