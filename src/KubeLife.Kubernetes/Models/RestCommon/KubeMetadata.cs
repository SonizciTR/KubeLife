using System.Text.Json.Serialization;

namespace KubeLife.Kubernetes.Models.RestCommon
{
    public class KubeMetadata
    {
        public string name { get; set; }
        [JsonPropertyName("namespace")]
        public string _namespace { get; set; }
        public string uid { get; set; }
        public string resourceVersion { get; set; }
        public DateTime creationTimestamp { get; set; }
        public Dictionary<string, string> labels { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> annotations { get; set; } = new Dictionary<string, string>();
        public KubeManagedfield[] managedFields { get; set; }
        public KubeOwnerreference[] ownerReferences { get; set; }
    }

}
