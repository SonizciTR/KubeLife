namespace KubeLife.Kubernetes.Models.RestCommon
{
    public class KubeCondition
    {
        public string type { get; set; }
        public string status { get; set; }
        public DateTime lastTransitionTime { get; set; }
    }

}
