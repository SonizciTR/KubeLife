using KubeLife.Kubernetes.Models.RestCommon;

namespace KubeLife.Kubernetes.Models.Routes
{
    public class KubeRouteItem
    {
        public KubeMetadata metadata { get; set; }
        public KubeSpec spec { get; set; }
        public KubeStatus status { get; set; }
    }

}
