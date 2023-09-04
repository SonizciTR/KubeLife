using KubeLife.Kubernetes.Models.RestCommon;

namespace KubeLife.Kubernetes.Models.Routes
{
    public class KubeRouteItem
    {
        public RawKubeMetadata metadata { get; set; }
        public KubeSpec spec { get; set; }
        public RawKubeIngressStatus status { get; set; }
    }

}
