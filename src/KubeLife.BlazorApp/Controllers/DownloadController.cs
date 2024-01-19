using KubeLife.Data.Services;
using KubeLife.DataCenter;
using KubeLife.Kubernetes.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KubeLife.BlazorApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DownloadController : ControllerBase
    {
        private readonly IKubeS3Factory s3FactService;
        private readonly KubeConfigModel config;

        public DownloadController(IKubeS3Factory s3FactService, KubeConfigModel config)
        {
            this.s3FactService = s3FactService;
            this.config = config;
        }

        [HttpGet("s3/{bucket}/{folder}/{filename}")]
        public async Task<ActionResult> DownloadArrayAsync(string bucket, string folder, string filename)
        {
            var mimeType = "application/octet-stream";

            string tmpFilePath = $"{folder}/{filename}";

            var s3Service = this.s3FactService.Get(DataCenter.Models.S3Options.Huawei);
            var s3Config = new DataCenter.Models.KubeS3Configuration
            {
                AccessKey = config.S3ModelAccessKey,
                //Endpoint = config.S3ModelEndpoint,
                Endpoint = $"https://{config.S3ModelEndpoint}/",
                SecretKey = config.S3ModelSecretKey,
                UseHttps = true
            };
            
            var isConnResult = await s3Service.Initialize(s3Config);
            var req = new DataCenter.Models.S3RequestGet
            {
                BucketName = bucket,
                FileName = tmpFilePath
            };

            var response = await s3Service.GetObject(req);
            var fileRslt = response.IsSuccess ? response.Result : null;

            if (fileRslt == null)
                return NotFound();

            return new FileContentResult(fileRslt, mimeType)
            {
                FileDownloadName = filename
            };
        }

        [HttpGet("s3stream/{bucket}/{folder}/{filename}")]
        public async Task<ActionResult> DownloadStreamAsync(string bucket, string folder, string filename)
        {
            var mimeType = "application/octet-stream";

            string tmpFilePath = $"{folder}/{filename}";

            var s3Service = this.s3FactService.Get(DataCenter.Models.S3Options.Huawei);
            var s3Config = new DataCenter.Models.KubeS3Configuration
            {
                AccessKey = config.S3ModelAccessKey,
                //Endpoint = config.S3ModelEndpoint,
                Endpoint = $"https://{config.S3ModelEndpoint}/",
                SecretKey = config.S3ModelSecretKey,
                UseHttps = true
            };

            var isConnResult = await s3Service.Initialize(s3Config);
            var req = new DataCenter.Models.S3RequestGet
            {
                BucketName = bucket,
                FileName = tmpFilePath
            };

            var response = await s3Service.GetStream(req);
            var fileRslt = response.IsSuccess ? response.Result : null;

            if (fileRslt == null)
                return NotFound();


            return new FileStreamResult(fileRslt, mimeType)
            {
                FileDownloadName = filename
            };
        }

    }
}
