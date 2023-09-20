using KubeLife.Core.Models;
using KubeLife.DataCenter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubeLife.DataCenter
{
    public interface IS3Service
    {
        Task<KubeLifeResult<string>> Initialize(KubeS3Configuration config);
        Task<KubeLifeResult<List<KubeS3Bucket>>> GetBuckets();
        Task<KubeLifeResult<string>> SaveObject(S3RequestCreate createInfo);
        Task<KubeLifeResult<string>> DeleteObject(S3RequestDelete deleteInfo);

    }
}
