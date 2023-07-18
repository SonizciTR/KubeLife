using KubeLife.Kubernetes.Models.RestCommon;

namespace KubeLife.Kubernetes.Models.Routes
{

    public class KubeCustomObjectforRoute : KubeCustomObjectBase
    {
        public List<KubeRouteItem> items { get; set; } = new List<KubeRouteItem>();
    }

}
