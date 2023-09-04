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

    public class Labels
    {
        public string app { get; set; }
        public string appbelongsto { get; set; }
        public string appkubernetesiomanagedby { get; set; }
        public string buildconfig { get; set; }
        public string openshiftiobuildconfigname { get; set; }
        public string openshiftiobuildstartpolicy { get; set; }
    }

    public class Annotations
    {
        public string openshiftiobuildconfigname { get; set; }
        public string openshiftiobuildnumber { get; set; }
        public string openshiftiobuildpodname { get; set; }
    }

    public class Ownerreference
    {
        public string apiVersion { get; set; }
        public string kind { get; set; }
        public string name { get; set; }
        public string uid { get; set; }
        public bool controller { get; set; }
    }

    public class Managedfield
    {
        public string manager { get; set; }
        public string operation { get; set; }
        public string apiVersion { get; set; }
        public DateTime time { get; set; }
        public string fieldsType { get; set; }
        public Fieldsv1 fieldsV1 { get; set; }
    }

    public class Fieldsv1
    {
        public FMetadata fmetadata { get; set; }
        public FSpec fspec { get; set; }
        public FStatus fstatus { get; set; }
    }

    public class FMetadata
    {
        public FAnnotations fannotations { get; set; }
        public FLabels flabels { get; set; }
        public FOwnerreferences fownerReferences { get; set; }
    }

    public class FAnnotations
    {
        public _ _ { get; set; }
        public FOpenshiftIoBuildConfigName fopenshiftiobuildconfigname { get; set; }
        public FOpenshiftIoBuildNumber fopenshiftiobuildnumber { get; set; }
        public FOpenshiftIoBuildPodName fopenshiftiobuildpodname { get; set; }
    }

    public class _
    {
    }

    public class FOpenshiftIoBuildConfigName
    {
    }

    public class FOpenshiftIoBuildNumber
    {
    }

    public class FOpenshiftIoBuildPodName
    {
    }

    public class FLabels
    {
        public _1 _ { get; set; }
        public FApp fapp { get; set; }
        public FAppBelongsTo fappbelongsto { get; set; }
        public FAppKubernetesIoManagedBy fappkubernetesiomanagedby { get; set; }
        public FBuildconfig fbuildconfig { get; set; }
        public FOpenshiftIoBuildConfigName1 fopenshiftiobuildconfigname { get; set; }
        public FOpenshiftIoBuildStartPolicy fopenshiftiobuildstartpolicy { get; set; }
    }

    public class _1
    {
    }

    public class FApp
    {
    }

    public class FAppBelongsTo
    {
    }

    public class FAppKubernetesIoManagedBy
    {
    }

    public class FBuildconfig
    {
    }

    public class FOpenshiftIoBuildConfigName1
    {
    }

    public class FOpenshiftIoBuildStartPolicy
    {
    }

    public class FOwnerreferences
    {
        public _2 _ { get; set; }
        public KUidF088bf7f34C94Da8A50bDefe4342af25 kuidf088bf7f34c94da8a50bdefe4342af25 { get; set; }
    }

    public class _2
    {
    }

    public class KUidF088bf7f34C94Da8A50bDefe4342af25
    {
    }

    public class FSpec
    {
        public FOutput foutput { get; set; }
        public FServiceaccount fserviceAccount { get; set; }
        public FSource fsource { get; set; }
        public FStrategy fstrategy { get; set; }
        public FTriggeredby ftriggeredBy { get; set; }
    }

    public class FOutput
    {
        public FTo fto { get; set; }
        public FPushsecret fpushSecret { get; set; }
    }

    public class FTo
    {
    }

    public class FPushsecret
    {
    }

    public class FServiceaccount
    {
    }

    public class FSource
    {
        public FContextdir fcontextDir { get; set; }
        public FGit fgit { get; set; }
        public FSourcesecret fsourceSecret { get; set; }
        public FType ftype { get; set; }
    }

    public class FContextdir
    {
    }

    public class FGit
    {
        public _3 _ { get; set; }
        public FUri furi { get; set; }
    }

    public class _3
    {
    }

    public class FUri
    {
    }

    public class FSourcesecret
    {
    }

    public class FType
    {
    }

    public class FStrategy
    {
        public FDockerstrategy fdockerStrategy { get; set; }
        public FType1 ftype { get; set; }
    }

    public class FDockerstrategy
    {
        public _4 _ { get; set; }
        public FDockerfilepath fdockerfilePath { get; set; }
    }

    public class _4
    {
    }

    public class FDockerfilepath
    {
    }

    public class FType1
    {
    }

    public class FTriggeredby
    {
    }

    public class FStatus
    {
        public FConditions fconditions { get; set; }
        public FConfig fconfig { get; set; }
        public FCompletiontimestamp fcompletionTimestamp { get; set; }
        public FDuration fduration { get; set; }
        public FOutputdockerimagereference foutputDockerImageReference { get; set; }
        public FPhase fphase { get; set; }
        public FStarttimestamp fstartTimestamp { get; set; }
    }

    public class FConditions
    {
        public _5 _ { get; set; }
        public KTypeNew ktypeNew { get; set; }
    }

    public class _5
    {
    }

    public class KTypeNew
    {
        public _6 _ { get; set; }
        public FLasttransitiontime flastTransitionTime { get; set; }
        public FLastupdatetime flastUpdateTime { get; set; }
        public FStatus1 fstatus { get; set; }
        public FType2 ftype { get; set; }
    }

    public class _6
    {
    }

    public class FLasttransitiontime
    {
    }

    public class FLastupdatetime
    {
    }

    public class FStatus1
    {
    }

    public class FType2
    {
    }

    public class FConfig
    {
    }

    public class FCompletiontimestamp
    {
    }

    public class FDuration
    {
    }

    public class FOutputdockerimagereference
    {
    }

    public class FPhase
    {
    }

    public class FStarttimestamp
    {
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
