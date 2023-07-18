namespace KubeLife.Kubernetes.Models.RestCommon
{
    public class KubeOwnerreference
    {
        public string apiVersion { get; set; }
        public string kind { get; set; }
        public string name { get; set; }
        public string uid { get; set; }
        public bool controller { get; set; }
        public bool blockOwnerDeletion { get; set; }
    }
}
