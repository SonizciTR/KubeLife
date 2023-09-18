using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubeLife.DataDomain.Models
{
    public class KubeS3Configuration
    {
        public string Endpoint { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public bool UseHttps { get; set; } = true;
    }
}
