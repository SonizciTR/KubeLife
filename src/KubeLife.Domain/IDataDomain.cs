using KubeLife.Core.Models;
using KubeLife.Domain.Models.Data;

namespace KubeLife.Domain
{
    public interface IDataDomain
    {
        Task<KubeLifeResult<S3BenchmarkContainer>> StartS3Benchmark(S3BenchmarkRequest benchmarkDetail);
    }
}
