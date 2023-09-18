using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using KubeLife.Core.Models;
using KubeLife.DataDomain;
using KubeLife.DataDomain.Models;
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
            if (!isInitialized) return new KubeLifeResult<List<DataDomain.Models.KubeS3Bucket>>(false, "Please initialize before use.");

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
    }
}
