using KubeLife.Data.S3;
using KubeLife.DataCenter;
using KubeLife.DataCenter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubeLife.Data.Services
{
    public interface IKubeS3Factory
    {
        IS3Service Get(S3Options selection);
        IS3Service Get(string name);
    }

    public class KubeS3Factory : IKubeS3Factory
    {
        public IS3Service Get(S3Options selection)
        {
            switch (selection)
            {
                case S3Options.Minio: return new S3MinioService();
                case S3Options.Huawei: return new S3HuaweiOBSService();
                case S3Options.AWS: return new S3AWSService();
            }
            
            return new S3MinioService();
        }

        public IS3Service Get(string name)
        {
            var tmp = name.ToLowerInvariant();
            switch (tmp)
            {
                case "minio": return new S3MinioService();
                case "huawei": return new S3HuaweiOBSService();
                case "aws": return new S3AWSService();
            }

            return new S3MinioService();
        }
    }
}
