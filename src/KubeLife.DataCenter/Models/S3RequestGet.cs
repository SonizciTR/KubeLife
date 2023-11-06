using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubeLife.DataCenter.Models
{
    public class S3RequestGet
    {
        public string BucketName { get; set; }
        public string ObjectKey { get; set; }
    }
}
