using KubeLife.Core.Models;
using KubeLife.DataDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubeLife.DataDomain
{
    public interface IS3Service
    {
        Task<KubeLifeResult<string>> Initialize(string endpoint, string accessKey, string secretKey, bool useHttps = true);
        Task<KubeLifeResult<List<S3Bucket>>> GetBuckets();
    }
}
