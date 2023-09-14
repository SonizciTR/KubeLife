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

        public async Task<KubeLifeResult<string>> Initialize(string endpoint, string accessKey, string secretKey, bool useHttps = true)
        {
            obsClient = new ObsClient(accessKey, secretKey, endpoint);

            return new KubeLifeResult<string>(true, "Success");
        }

        public async Task<KubeLifeResult<List<S3BucketInfo>>> GetBuckets()
        {
            if (!isInitialized) return new KubeLifeResult<List<S3BucketInfo>>(false, "Please initialize before use.");

            var target = new List<S3BucketInfo>();

            ListBucketsRequest request = new ListBucketsRequest();
            ListBucketsResponse response = obsClient.ListBuckets(request);
            foreach (ObsBucket bucket in response.Buckets)
            {
                var tmp = new S3BucketInfo
                {
                    Name = bucket.BucketName,
                    CreatedDate = bucket.CreationDate ?? DateTime.MinValue,
                };
                target.Add(tmp);
            }

            return new KubeLifeResult<List<S3BucketInfo>>(target);
        }
    }
}
