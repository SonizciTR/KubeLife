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
    }

    public class S3BenchmarkResult
    {
        public List<Exception> Errors { get; internal set; } = new List<Exception>();
        public double TimeTotalSec { get; internal set; }
        public double TimePerFileSec { get; internal set; }
        public double TimePerItemMs { get; internal set; }
        public int FileSizeKB { get; internal set; }
        public int SuccessCount { get; internal set; }
        public int ErrorCount { get; internal set; }
    }
}
