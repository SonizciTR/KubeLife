namespace KubeLife.Kubernetes.Models.Routes
{
    public class KubeSpec
    {
        public string host { get; set; }
        public KubeTo to { get; set; }
        public KubePort port { get; set; }
        public KubeTls tls { get; set; }
        public string wildcardPolicy { get; set; }
        public string path { get; set; }
    }

}
