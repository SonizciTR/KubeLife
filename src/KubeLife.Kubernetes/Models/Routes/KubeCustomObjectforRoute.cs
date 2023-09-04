using KubeLife.Kubernetes.Models.RestCommon;

namespace KubeLife.Kubernetes.Models.Routes
{

    public class KubeCustomObjectforRoute : RawKubeCustomObjectBase
    {
        public List<KubeRouteItem> items { get; set; } = new List<KubeRouteItem>();
    }

}
