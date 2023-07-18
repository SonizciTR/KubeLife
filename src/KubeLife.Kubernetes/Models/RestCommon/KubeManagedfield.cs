namespace KubeLife.Kubernetes.Models.RestCommon
{
    public class KubeManagedfield
    {
        public string manager { get; set; }
        public string operation { get; set; }
        public string apiVersion { get; set; }
        public DateTime time { get; set; }
        public string fieldsType { get; set; }
        public string subresource { get; set; }
    }

}
