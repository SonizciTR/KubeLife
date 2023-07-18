namespace KubeLife.Kubernetes.Models.RestCommon
{
    public class KubeTls
    {
        public string termination { get; set; }
        public string insecureEdgeTerminationPolicy { get; set; }
        public string destinationCACertificate { get; set; }
    }

}
