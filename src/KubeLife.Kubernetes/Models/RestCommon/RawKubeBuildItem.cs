using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubeLife.Kubernetes.Models.RestCommon
{
    public class RawKubeBuildMain : RawKubeCustomObjectBase
    {
        public List<RawKubeBuildItem> items { get; set; }
    }

    public class RawKubeBuildItem
    {
        public RawKubeMetadata metadata { get; set; }
        public RawKubeSpec spec { get; set; }
        public RawKubeBuildStatus status { get; set; }
    }

    public class RawKubeSpec
    {
        public string serviceAccount { get; set; }
        public RawKubeSource source { get; set; }
        public RawKubeRevision revision { get; set; }
        public RawKubeStrategy strategy { get; set; }
        public RawKubeOutput output { get; set; }
        public RawKubeResources resources { get; set; }
        public RawKubePostcommit postCommit { get; set; }
        public object nodeSelector { get; set; }
        public RawKubeTriggeredby[] triggeredBy { get; set; }
    }

    public class RawKubeSource
    {
        public string type { get; set; }
        public RawKubeGit git { get; set; }
        public string contextDir { get; set; }
        public RawKubeSourcesecret sourceSecret { get; set; }
    }

    public class RawKubeGit
    {
        public string uri { get; set; }
        public string commit { get; set; }
        public RawKubeAuthor author { get; set; }
        public RawKubeCommitter committer { get; set; }
        public string message { get; set; }
    }

    public class RawKubeSourcesecret
    {
        public string name { get; set; }
    }

    public class RawKubeRevision
    {
        public string type { get; set; }
        public RawKubeGit git { get; set; }
    }

    public class RawKubeAuthor
    {
        public string name { get; set; }
        public string email { get; set; }
    }

    public class RawKubeCommitter
    {
        public string name { get; set; }
        public string email { get; set; }
    }

    public class RawKubeStrategy
    {
        public string type { get; set; }
        public RawKubeDockerstrategy dockerStrategy { get; set; }
    }

    public class RawKubeDockerstrategy
    {
        public string dockerfilePath { get; set; }
    }

    public class RawKubeOutput
    {
        public KubeTo to { get; set; }
        public RawKubePushsecret pushSecret { get; set; }
    }

    public class RawKubePushsecret
    {
        public string name { get; set; }
    }

    public class RawKubeResources
    {
    }

    public class RawKubePostcommit
    {
    }

    public class RawKubeTriggeredby
    {
        public string message { get; set; }
    }

    public class RawKubeBuildStatus
    {
        public string phase { get; set; }
        public DateTime startTimestamp { get; set; }
        public DateTime completionTimestamp { get; set; }
        public long duration { get; set; }
        public string outputDockerImageReference { get; set; }
        public RawKubeConfig config { get; set; }
        public RawKubeOutput1 output { get; set; }
        public RawKubeStage[] stages { get; set; }
        public KubeCondition[] conditions { get; set; }
    }

    public class RawKubeConfig
    {
        public string kind { get; set; }
        public string _namespace { get; set; }
        public string name { get; set; }
    }

    public class RawKubeOutput1
    {
        public KubeTo to { get; set; }
    }

    public class RawKubeStage
    {
        public string name { get; set; }
        public DateTime startTime { get; set; }
        public int durationMilliseconds { get; set; }
        public RawKubeStep[] steps { get; set; }
    }

    public class RawKubeStep
    {
        public string name { get; set; }
        public DateTime startTime { get; set; }
        public int durationMilliseconds { get; set; }
    }
}
