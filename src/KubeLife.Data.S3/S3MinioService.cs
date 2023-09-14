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

        public async Task<KubeLifeResult<string>> Initialize(string endpoint, string accessKey, string secretKey, bool useHttps = true)
        {
            minioClient = new MinioClient()
                                    .WithEndpoint(endpoint)
                                    .WithCredentials(accessKey, secretKey)
                                    .WithSSL(useHttps)
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

        public async Task<KubeLifeResult<List<S3BucketInfo>>> GetBuckets()
        {
            if (!isInitialized) return new KubeLifeResult<List<S3BucketInfo>>(false, "Please initialize before use.");

            var bckts = await minioClient.ListBucketsAsync();
            var target = new List<S3BucketInfo>();
            foreach (var bucket in bckts.Buckets)
            {
                var tmp = new S3BucketInfo();
                tmp.Name = bucket.Name;
                tmp.CreatedDate = bucket.CreationDateDateTime;
                target.Add(tmp);
            }

            return new KubeLifeResult<List<S3BucketInfo>>(target);
        }
    }
}