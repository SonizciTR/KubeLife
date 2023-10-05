using k8s.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubeLife.Domain.Models.Data
{
    public class S3BenchmarkContainer
    {
        public S3BenchmarkResult SaveResult { get; set; }
        public S3BenchmarkResult DeleteResult { get; set; }
        public S3BenchmarkResult ReadResult { get; set; }
        public double FileSizeKB { get; internal set; }
        public double FileSizeMB => (double)FileSizeKB / 1024;
    }
}
