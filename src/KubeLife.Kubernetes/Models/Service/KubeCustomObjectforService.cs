using KubeLife.Kubernetes.Models.RestCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubeLife.Kubernetes.Models.Service
{
    public class KubeCustomObjectforService : KubeCustomObjectBase
    {
        public KubeServiceSpec spec { get; set; }
    }
}
