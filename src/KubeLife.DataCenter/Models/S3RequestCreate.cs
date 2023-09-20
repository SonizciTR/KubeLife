using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubeLife.DataCenter.Models
{
    public class S3RequestCreate
    {
        public string BucketName { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] ContentData { get; set; }
    }
}
