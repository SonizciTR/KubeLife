﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubeLife.Kubernetes.Models
{
    public class KubeServiceModel
    {
        public string HostName { get; set; }
        public string Namespace { get; set; }
        public string ServiceName { get; set; }
        public string ServicePortName { get; set; }
        public string TlsTermination { get; set; }
        public string WildcardPolicy { get; set; }
        public IDictionary<string, string> Selector { get; internal set; }
        public string ClusterIP { get; internal set; }
        public string InternalTrafficPolicy { get; internal set; }
    }
}
