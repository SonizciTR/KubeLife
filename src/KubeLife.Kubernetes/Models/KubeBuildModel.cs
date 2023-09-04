namespace KubeLife.Kubernetes.Models
{
    public class KubeBuildModel
    {
        public string BuildName { get; set; }
        public string Namespace { get; set; }
        public DateTime CreateDate { get; set; }
        public string StatusPhase { get; set; }
        public DateTime CompletationTime { get; set; }
    }
}
