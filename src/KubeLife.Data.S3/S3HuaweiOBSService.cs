using KubeLife.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OBS;
using OBS.Model;
using KubeLife.DataCenter;
using KubeLife.DataCenter.Models;
using System.IO;
using KubeLife.Core.Extensions;

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

        public async Task<KubeLifeResult<string>> SaveObject(S3RequestCreate createInfo)
        {
            var req = new PutObjectRequest();
            req.BucketName = createInfo.BucketName;
            req.ContentType = createInfo.ContentType;
            req.ObjectKey = createInfo.FileName;
            req.InputStream = new MemoryStream(createInfo.ContentData);

            var resp = obsClient.PutObject(req);
            bool isSucc = resp.StatusCode == System.Net.HttpStatusCode.OK;
            string respBody = resp.OriginalResponse.HttpWebResponse.GetResponseStream().ToStringRaw();

            return new KubeLifeResult<string>(isSucc, respBody);
        }

        public async Task<KubeLifeResult<string>> DeleteObject(S3RequestDelete deleteInfo)
        {
            var req = new DeleteObjectRequest() { };
            req.BucketName = deleteInfo.BucketName;
            req.ObjectKey = deleteInfo.FileName;

            var resp = obsClient.DeleteObject(req);

            bool isSucc = resp.StatusCode == System.Net.HttpStatusCode.OK;
            string respBody = resp.OriginalResponse.HttpWebResponse.GetResponseStream().ToStringRaw();

            return new KubeLifeResult<string>(isSucc, respBody);
        }
    }
}
