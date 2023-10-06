using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubeLife.Domain.Models.Data
{
    public class S3BenchmarkRequest
    {
        public string S3TypeSelection { get; set; }
        public int RecordCount { get; set; }
        public int RepeatCount { get; set; }
        public string BucketName { get; set; }
        public string S3AccessKey { get; set; }
        public string S3SecretKey { get; set; }
        public string S3Endpoint { get; set; }
    }
}
