namespace KubeLife.Kubernetes.Models.Routes
{
    public class KubeIngress
    {
        public string host { get; set; }
        public string routerName { get; set; }
        public KubeCondition[] conditions { get; set; }
        public string wildcardPolicy { get; set; }
        public string routerCanonicalHostname { get; set; }
    }

}
