namespace KubeLife.Kubernetes.Models.RestCommon
{
    public abstract class KubeCustomObjectBase
    {
        public string kind { get; set; }
        public string apiVersion { get; set; }
        public KubeMetadata metadata { get; set; }
        public KubeStatus status { get; set; }
    }

}
