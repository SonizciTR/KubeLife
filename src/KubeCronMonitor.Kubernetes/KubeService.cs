using k8s;

namespace KubeCronMonitor.Kubernetes
{
    public class KubeService : IKubeService
    {
        public KubeService(string serverUrl, string accessToken, string userName, string passWord)
        {
            ServerUrl = serverUrl;
            AccessToken = accessToken;
            UserName = userName;
            PassWord = passWord;
        }

        public string ServerUrl { get; }
        public string AccessToken { get; }
        public string UserName { get; }
        public string PassWord { get; }

        public async Task<bool> Get()
        {
            var config = new KubernetesClientConfiguration();
            config.Host = ServerUrl;
            config.Password = PassWord;
            config.Username = UserName;
            config.AccessToken = AccessToken;

            var client = new k8s.Kubernetes(config);

            var namespaces = await client.ListCronJobForAllNamespacesAsync();

            return true;
        }
    }
}