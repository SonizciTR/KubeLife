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
        Task<KubeLifeResult<string>> Initialize(KubeS3Configuration config);
        Task<KubeLifeResult<List<KubeS3Bucket>>> GetBuckets();
    }
}
