using KubeLife.Core.Models;
using KubeLife.Data.S3.Model;
using KubeLife.DataCenter;
using KubeLife.DataCenter.Models;
using Minio;
using System;
using System.Security.AccessControl;
using System.Text;
using System.Xml.Linq;
using KubeLife.Core.Extensions;
using Minio.DataModel.Args;

namespace KubeLife.Data.S3
{
    public class S3MinioService : IS3Service
    {
        private bool isInitialized = false;
        private MinioClient minioClient = null;

        public async Task<KubeLifeResult<string>> Initialize(KubeS3Configuration config)
        {
            minioClient = (MinioClient)new MinioClient()
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

        public async Task<KubeLifeResult<byte[]>> GetObject(S3RequestGet fileGetInfo)
        {
            var bufferStream = new MemoryStream();
            var arg = new GetObjectArgs()
                .WithBucket(fileGetInfo.BucketName)
                .WithObject(fileGetInfo.FileName)
                .WithCallbackStream((stream) => stream.CopyTo(bufferStream));

            var resp = await minioClient.GetObjectAsync(arg);

            bool isSucc = !string.IsNullOrWhiteSpace(resp.ETag);
            var respBody = bufferStream.ToArray();

            return new KubeLifeResult<byte[]>(isSucc, $"Error Etag : {resp.ETag}", respBody);

            //var statArgs = new StatObjectArgs()
            //                    .WithObject(fileGetInfo.FileName)
            //                    .WithBucket(fileGetInfo.BucketName);
            //var stat = await minioClient.StatObjectAsync(statArgs);

            //var res = new MinioReleaseableFileStreamModel
            //{
            //    ContentType = stat.ContentType,
            //    FileName = fileGetInfo.FileName,
            //};

            //// the magic begins here
            //var getArgs = new GetObjectArgs()
            //    .WithObject(fileGetInfo.FileName)
            //    .WithBucket(fileGetInfo.BucketName)
            //    .WithCallbackStream(res.SetStreamAsync);

            //await res.HandleAsync(minioClient.GetObjectAsync(getArgs));
            //// the magic partially ends here
            //var respBody = res.StreamHolder.ToByteArray();

            //return new KubeLifeResult<byte[]>(respBody?.Length > 0, $"Data Length : {respBody?.Length}", respBody);
        }

        public async Task<KubeLifeResult<Stream>> GetStream(S3RequestGet fileGetInfo)
        {
            var bufferStream = new MemoryStream();
            var arg = new GetObjectArgs()
                .WithBucket(fileGetInfo.BucketName)
                .WithObject(fileGetInfo.FileName)
                .WithCallbackStream((stream) => stream.CopyTo(bufferStream));

            var resp = await minioClient.GetObjectAsync(arg);

            bool isSucc = !string.IsNullOrWhiteSpace(resp.ETag);

            return new KubeLifeResult<Stream>(isSucc, $"Error Etag : {resp.ETag}", bufferStream);
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

            return new KubeLifeResult<string>(isSucc, $"Error Etag : {resp.Etag}", respBody);
        }

        public async Task<KubeLifeResult<string>> DeleteObject(S3RequestDelete deleteInfo)
        {
            var args = new RemoveObjectArgs()
               .WithBucket(deleteInfo.BucketName)
               .WithObject(deleteInfo.FileName);

            await minioClient.RemoveObjectAsync(args);

            bool isSucc = true;
            string respBody = $"Deleted : {deleteInfo.FileName}";

            return new KubeLifeResult<string>(isSucc, $"No error code could be returned.", respBody);
        }
    }
}