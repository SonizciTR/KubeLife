namespace KubeLife.Kubernetes.Models.RestCommon
{
    public abstract class RawKubeCustomObjectBase
    {
        public string kind { get; set; }
        public string apiVersion { get; set; }
        public RawKubeMetadata metadata { get; set; }
        public RawKubeIngressStatus status { get; set; }
    }

}
