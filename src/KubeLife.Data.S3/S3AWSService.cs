using Amazon;
using Amazon.Runtime.Internal;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using KubeLife.Core.Extensions;
using KubeLife.Core.Models;
using KubeLife.DataCenter;
using KubeLife.DataCenter.Models;
using Minio;
using Minio.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace KubeLife.Data.S3
{
    public class S3AWSService : IS3Service
    {
        private bool isInitialized = false;
        private IAmazonS3 awsClient = null;

        public async Task<KubeLifeResult<string>> Initialize(KubeS3Configuration config)
        {
            awsClient = GetClientByRegion(config);
            if (awsClient == null)
            {
                AmazonS3Config cnfAws = new AmazonS3Config();
                cnfAws.ServiceURL = config.Endpoint;
                awsClient = new AmazonS3Client(config.AccessKey, config.SecretKey, cnfAws);
            }
            
            var isExist = await AmazonS3Util.DoesS3BucketExistV2Async(awsClient, "nonexistendbucket");
            if (!isExist)
            {
                isInitialized = true;
                return new KubeLifeResult<string>(true, "Success.");
            }

            return new KubeLifeResult<string>(false, "Could not initialized AWS connection to S3.");
        }

        private IAmazonS3 GetClientByRegion(KubeS3Configuration config)
        {
            try
            {
                var regionIdentifier = RegionEndpoint.GetBySystemName(config.Endpoint);
                return new AmazonS3Client(config.AccessKey, config.SecretKey, regionIdentifier);
            }
            catch
            {
            }

            return null;
        }

        public async Task<KubeLifeResult<List<KubeS3Bucket>>> GetBuckets()
        {
            if (!isInitialized) return new KubeLifeResult<List<KubeS3Bucket>>(false, "Please initialize before use.");

            ListBucketsResponse response = await awsClient.ListBucketsAsync();

            var target = new List<KubeS3Bucket>();
            foreach (S3Bucket bucket in response.Buckets)
            {
                var tmp = new KubeS3Bucket();
                tmp.Name = bucket.BucketName;
                tmp.CreatedDate = bucket.CreationDate;
                target.Add(tmp);
            }

            return new KubeLifeResult<List<KubeS3Bucket>>(target);
        }

        public async Task<KubeLifeResult<byte[]>> GetObject(S3RequestGet fileGetInfo)
        {
            var req = new GetObjectRequest
            {
                BucketName = fileGetInfo.BucketName,
                Key = fileGetInfo.FileName
            };
            var resp = await awsClient.GetObjectAsync(req);
            bool isSucc = resp.HttpStatusCode == System.Net.HttpStatusCode.OK;
            byte[] fileArray = null;
            if(isSucc)
                fileArray = resp.ResponseStream.ToByteArray();

            return new KubeLifeResult<byte[]>(isSucc, $"Error Code : {resp.HttpStatusCode}", fileArray);
        }

        public async Task<KubeLifeResult<string>> SaveObject(S3RequestCreate createInfo)
        {
            PutObjectRequest request = new PutObjectRequest
            {
                BucketName = createInfo.BucketName,
                Key = createInfo.FileName,
                ContentBody = Encoding.UTF8.GetString(createInfo.ContentData)
            };

            var resp = await awsClient.PutObjectAsync(request);
            bool isSucc = resp.HttpStatusCode == System.Net.HttpStatusCode.OK;
            string respBody = resp.ETag;

            return new KubeLifeResult<string>(isSucc, $"Error Code : {resp.HttpStatusCode}", respBody);
        }

        public async Task<KubeLifeResult<string>> DeleteObject(S3RequestDelete deleteInfo)
        {
            var req = new DeleteObjectRequest();
            req.BucketName = deleteInfo.BucketName;
            req.Key = deleteInfo.FileName;
            
            var resp = await awsClient.DeleteObjectAsync(req);
            bool isSucc = resp.HttpStatusCode == System.Net.HttpStatusCode.OK;
            string respBody = resp.DeleteMarker;

            return new KubeLifeResult<string>(isSucc, $"Error Code : {resp.HttpStatusCode}", respBody);
        }

        
    }
}
