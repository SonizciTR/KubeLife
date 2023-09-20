using KubeLife.Core.Models;
using KubeLife.DataCenter;
using KubeLife.DataCenter.Models;
using Minio;
using System;
using System.Security.AccessControl;
using System.Text;

namespace KubeLife.Data.S3
{
    public class S3MinioService : IS3Service
    {
        private bool isInitialized = false;
        private MinioClient minioClient = null;

        public async Task<KubeLifeResult<string>> Initialize(KubeS3Configuration config)
        {
            minioClient = new MinioClient()
                                    .WithEndpoint(config.Endpoint)
                                    .WithCredentials(config.AccessKey, config.SecretKey)
                                    .WithSSL(config.UseHttps)
                                    .Build();

            var beArgs = new BucketExistsArgs().WithBucket("notexistbucket");
            bool found = await minioClient.BucketExistsAsync(beArgs);

            if (minioClient != null)
            {
                isInitialized = true;
                return new KubeLifeResult<string>("Success");
            }

            return new KubeLifeResult<string>(false, "Could not initialized Minio connection to S3.");
        }

        public async Task<KubeLifeResult<List<KubeS3Bucket>>> GetBuckets()
        {
            if (!isInitialized) return new KubeLifeResult<List<KubeS3Bucket>>(false, "Please initialize before use.");

            var bckts = await minioClient.ListBucketsAsync();
            var target = new List<KubeS3Bucket>();
            foreach (var bucket in bckts.Buckets)
            {
                var tmp = new KubeS3Bucket();
                tmp.Name = bucket.Name;
                tmp.CreatedDate = bucket.CreationDateDateTime;
                target.Add(tmp);
            }

            return new KubeLifeResult<List<KubeS3Bucket>>(target);
        }

        public async Task<KubeLifeResult<string>> SaveObject(S3RequestCreate createInfo)
        {
            var data = Encoding.UTF8.GetString(createInfo.ContentData);
            var args = new PutObjectArgs()
            .WithBucket(createInfo.BucketName)
                .WithObject(data)
                .WithContentType(createInfo.ContentType)
                .WithFileName(createInfo.FileName);
            
            var resp = await minioClient.PutObjectAsync(args);

            bool isSucc = !string.IsNullOrWhiteSpace(resp.Etag);
            string respBody = resp.ObjectName;

            return new KubeLifeResult<string>(isSucc, respBody);
        }

        public async Task<KubeLifeResult<string>> DeleteObject(S3RequestDelete deleteInfo)
        {
            var args = new RemoveObjectArgs()
               .WithBucket(deleteInfo.BucketName)
               .WithObject(deleteInfo.FileName);

            await minioClient.RemoveObjectAsync(args);

            bool isSucc = true;
            string respBody = $"Deleted : {deleteInfo.FileName}";

            return new KubeLifeResult<string>(isSucc, respBody);
        }
    }
}