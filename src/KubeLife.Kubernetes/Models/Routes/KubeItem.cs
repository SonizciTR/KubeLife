namespace KubeLife.Kubernetes.Models.Routes
{
    public class KubeItem
    {
        public KubeMetadata1 metadata { get; set; }
        public KubeSpec spec { get; set; }
        public KubeStatus status { get; set; }
    }

}
