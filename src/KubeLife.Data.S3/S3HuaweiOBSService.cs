using KubeLife.Core.Models;
using KubeLife.DataDomain;
using KubeLife.DataDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OBS;
using OBS.Model;

namespace KubeLife.Data.S3
{
    public class S3HuaweiOBSService : IS3Service
    {
        private bool isInitialized = false;
        private ObsClient obsClient = null;

        public async Task<KubeLifeResult<string>> Initialize(KubeS3Configuration config)
        {
            obsClient = new ObsClient(config.AccessKey, config.SecretKey, config.Endpoint);

            return new KubeLifeResult<string>(true, "Success");
        }

        public async Task<KubeLifeResult<List<KubeS3Bucket>>> GetBuckets()
        {
            if (!isInitialized) return new KubeLifeResult<List<KubeS3Bucket>>(false, "Please initialize before use.");

            var target = new List<KubeS3Bucket>();

            ListBucketsRequest request = new ListBucketsRequest();
            ListBucketsResponse response = obsClient.ListBuckets(request);
            foreach (ObsBucket bucket in response.Buckets)
            {
                var tmp = new KubeS3Bucket
                {
                    Name = bucket.BucketName,
                    CreatedDate = bucket.CreationDate ?? DateTime.MinValue,
                };
                target.Add(tmp);
            }

            return new KubeLifeResult<List<KubeS3Bucket>>(target);
        }
    }
}
