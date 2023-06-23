using k8s;
using KubeCronMonitor.Kubernetes.Models;

namespace KubeCronMonitor.Kubernetes
{
    public class KubeService : IKubeService
    {
        public KubeService(KubeConfigModel settings)
        {
            Settings = settings;
        }

        public KubeConfigModel Settings { get; }

        public async Task<bool> Get()
        {
            var config = new KubernetesClientConfiguration();
            config.Host = Settings.ServerUrl;
            config.Password = Settings.PassWord;
            config.Username = Settings.UserName;
            config.AccessToken = Settings.AccessToken;

            var client = new k8s.Kubernetes(config);

            var namespaces = await client.ListCronJobForAllNamespacesAsync();

            return true;
        }
    }
}