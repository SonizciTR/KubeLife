using KubeLife.DataCenter;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KubeLife.BlazorApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DownloadController : ControllerBase
    {
        private readonly IS3Service s3Service;

        //public DownloadController(IS3Service s3Service)
        //{
        //    this.s3Service = s3Service;
        //}

        [HttpGet("v1/{bucket}/{folder}/{model}")]
        public async Task<ActionResult> DownloadAsync(string bucket, string folder, string model)
        {
            var fileName = "myfileName.txt";
            var mimeType = "application/....";
            byte[] fileBytes = null;

            if(fileBytes == null)
                return NotFound();

            return new FileContentResult(fileBytes, mimeType)
            {
                FileDownloadName = fileName
            };
        }

    }
}
