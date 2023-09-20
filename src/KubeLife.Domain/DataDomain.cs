using k8s.Models;
using KubeLife.Core.Models;
using KubeLife.Data.Services;
using KubeLife.DataCenter;
using KubeLife.DataCenter.Helpers;
using KubeLife.DataCenter.Models;
using KubeLife.DataCenter.Helpers;
using KubeLife.Domain.Models.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KubeLife.Domain
{
    public interface IDataDomain
    {
    }

    public class DataDomain : IDataDomain
    {
        private readonly IKubeS3Factory s3Factory;

        public DataDomain(IKubeS3Factory kubeS3Factory)
        {
            this.s3Factory = kubeS3Factory;
        }

        public async Task<KubeLifeResult<S3BenchmarkResult>> StartS3Benchmark(S3BenchmarkRequest benchmarkDetail)
        {
            var response = new S3BenchmarkResult();
            var data = DataGenerator.GenerateData<S3BenchmarkData>(benchmarkDetail.RecordCount);
            var csvString = CsvSerializer.ToCsv(";", data);
            var csvBinary = Encoding.UTF8.GetBytes(csvString);

            IS3Service s3Service = s3Factory.Get(benchmarkDetail.S3TypeSelection);
            var rsltConnection = await ConnectS3(s3Service, benchmarkDetail);
            if (!rsltConnection.IsSuccess) return new KubeLifeResult<S3BenchmarkResult>(false, rsltConnection.Message);

            string verCode = $"{DateTime.Now.ToString("yyyMMddHHmmss")}";
            var filesCreated = new List<string>();
            var timeSaveStart = Stopwatch.StartNew();
            for (var i = 0; i < benchmarkDetail.RepeatCount; ++i)
            {
                string tmpFileName = $"TestFile.{verCode}.{i + 1}.csv";
                try
                {
                    var tmpReq = new S3RequestCreate
                    {
                        BucketName = benchmarkDetail.BucketName,
                        ContentData = csvBinary,
                        FileName = tmpFileName,
                        ContentType = "text/csv"
                    };

                    var tmpRslt = await s3Service.SaveObject(tmpReq);
                    filesCreated.Add(tmpFileName);
                }
                catch (Exception ex)
                {
                    response.Errors.Add(ex);
                }
            }
            timeSaveStart.Stop();

            await DeleteCreatedFiles(s3Service, benchmarkDetail, filesCreated);
            
            response.TimeTotalSec = timeSaveStart.Elapsed.TotalSeconds;
            response.TimePerFileSec = timeSaveStart.Elapsed.TotalSeconds / benchmarkDetail.RepeatCount;
            response.TimePerRowMs = timeSaveStart.Elapsed.TotalMilliseconds / (benchmarkDetail.RepeatCount * benchmarkDetail.RecordCount);

            response.FileSizeKB = csvBinary.Length / 1024;
            response.SuccessCount = benchmarkDetail.RepeatCount - response.Errors.Count;
            response.ErrorCount = response.Errors.Count;

            return new KubeLifeResult<S3BenchmarkResult>(response);
        }

        private async Task<bool> DeleteCreatedFiles(IS3Service s3StorageService, S3BenchmarkRequest benchmarkDetail, List<string> filesCreated)
        {
            bool isSucc = true;
            for (int i = 0; i < filesCreated.Count; i++)
            {
                try
                {

                    var tmpReq = new S3RequestDelete
                    {
                        BucketName = benchmarkDetail.BucketName,
                        FileName = filesCreated[i]
                    };
                    await s3StorageService.DeleteObject(tmpReq);
                }
                catch
                {
                    isSucc = false;
                }
            }

            return isSucc;
        }

        internal async Task<KubeLifeResult<S3BenchmarkResult>> ConnectS3(IS3Service s3StorageService, S3BenchmarkRequest benchmarkDetail)
        {
            try
            {
                var confS3 = new KubeS3Configuration();
                confS3.AccessKey = benchmarkDetail.S3AccessKey;
                confS3.SecretKey = benchmarkDetail.S3SecretKey;
                confS3.Endpoint = benchmarkDetail.S3Endpoint;

                var resultS3 = await s3StorageService.Initialize(confS3);
                if (!resultS3.IsSuccess) return new KubeLifeResult<S3BenchmarkResult>(false, resultS3.Message);

                return new KubeLifeResult<S3BenchmarkResult>(true, "");
            }
            catch (Exception ex)
            {
                return new KubeLifeResult<S3BenchmarkResult>(false, $"Exception on S3 connection : {ex.Message}");
            }
        }
    }
}
