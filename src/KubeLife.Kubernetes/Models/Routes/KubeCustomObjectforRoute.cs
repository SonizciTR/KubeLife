namespace KubeLife.Kubernetes.Models.Routes
{
    public class KubeCustomObjectforRoute
    {
        public string kind { get; set; }
        public string apiVersion { get; set; }
        public KubeMetadata metadata { get; set; }
        public List<KubeItem> items { get; set; } = new List<KubeItem>();
    }

}
