using KubeLife.Core.Models;
using KubeLife.DataDomain;
using KubeLife.DataDomain.Models;
using Minio;

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
    }
}