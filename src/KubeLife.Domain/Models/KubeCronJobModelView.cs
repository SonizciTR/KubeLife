using KubeLife.Kubernetes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubeLife.Domain.Models
{
    public class KubeCronJobModelView : KubeCronJobModel
    {
        public DateTime NextRunTime { get; set; }
    }
}
