namespace KubeLife.Kubernetes.Models.Routes
{
    public class KubeTls
    {
        public string termination { get; set; }
        public string insecureEdgeTerminationPolicy { get; set; }
        public string destinationCACertificate { get; set; }
    }

}
