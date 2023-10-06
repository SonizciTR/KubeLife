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
using IdentityModel.OidcClient;

namespace KubeLife.Domain
{
    public class DataDomain : IDataDomain
    {
        private readonly IKubeS3Factory s3Factory;

        public DataDomain(IKubeS3Factory kubeS3Factory)
        {
            this.s3Factory = kubeS3Factory;
        }

        public async Task<KubeLifeResult<S3BenchmarkContainer>> StartS3Benchmark(S3BenchmarkRequest benchmarkDetail)
        {
            var response = new S3BenchmarkContainer();
            var data = DataGenerator.GenerateData<S3BenchmarkData>(benchmarkDetail.RecordCount);
            var csvString = CsvSerializer.ToCsv(";", data);
            var csvBinary = Encoding.UTF8.GetBytes(csvString);

            IS3Service s3Service = s3Factory.Get(benchmarkDetail.S3TypeSelection);
            var rsltConnection = await ConnectS3(s3Service, benchmarkDetail);
            if (!rsltConnection.IsSuccess) return new KubeLifeResult<S3BenchmarkContainer>(false, rsltConnection.Message);

            response.FileSizeKB = (double)csvBinary.Length / 1024;
            var filesCreated = GetTestFileNames(benchmarkDetail);

            response.SaveResult = await RunSaveSenario(benchmarkDetail, csvBinary, s3Service, filesCreated);
            response.DeleteResult = await RunDeleteSenario(s3Service, benchmarkDetail, filesCreated);

            return new KubeLifeResult<S3BenchmarkContainer>(response);
        }

        internal List<string> GetTestFileNames(S3BenchmarkRequest benchmarkDetail)
        {
            var names = new List<string>();
            string verCode = $"{DateTime.Now.ToString("yyyMMddHHmmss")}";
            for (var i = 0; i < benchmarkDetail.RepeatCount; ++i)
            {
                string tmpFileName = $"TestFile.{verCode}.{i + 1}.csv";
                names.Add(tmpFileName);
            }
            return names;
        }

        internal async Task<S3BenchmarkResult> RunSaveSenario(S3BenchmarkRequest benchmarkDetail, byte[] csvBinary, IS3Service s3Service, List<string> filesCreated)
        {
            var result = new S3BenchmarkResult();
            var timeSaveStart = Stopwatch.StartNew();
            for (var i = 0; i < benchmarkDetail.RepeatCount; ++i)
            {
                try
                {
                    var tmpFileName = filesCreated[i];
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
                    result.Errors.Add(ex);
                }
            }
            result = SetTheTimingResult(benchmarkDetail, result, timeSaveStart);

            return result;
        }

        internal S3BenchmarkResult SetTheTimingResult(S3BenchmarkRequest benchmarkDetail, S3BenchmarkResult result, Stopwatch timeSaveStart)
        {
            if(timeSaveStart.IsRunning)
                timeSaveStart.Stop();

            result.TimeTotalSec = timeSaveStart.Elapsed.TotalSeconds;
            result.TimePerFileSec = timeSaveStart.Elapsed.TotalSeconds / benchmarkDetail.RepeatCount;
            result.TimePerItemMs = timeSaveStart.Elapsed.TotalMilliseconds / (benchmarkDetail.RepeatCount * benchmarkDetail.RecordCount);

            result.SuccessCount = benchmarkDetail.RepeatCount - result.Errors.Count;
            result.ErrorCount = result.Errors.Count;

            return result;
        }

        internal async Task<S3BenchmarkResult> RunDeleteSenario(IS3Service s3StorageService, S3BenchmarkRequest benchmarkDetail, List<string> filesCreated)
        {
            bool isSucc = true;
            var result = new S3BenchmarkResult();
            var timeSaveStart = Stopwatch.StartNew();

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

            timeSaveStart.Stop();

            result = SetTheTimingResult(benchmarkDetail, result, timeSaveStart);

            return result;
        }

        internal async Task<KubeLifeResult<S3BenchmarkContainer>> ConnectS3(IS3Service s3StorageService, S3BenchmarkRequest benchmarkDetail)
        {
            try
            {
                var confS3 = new KubeS3Configuration();
                confS3.AccessKey = benchmarkDetail.S3AccessKey;
                confS3.SecretKey = benchmarkDetail.S3SecretKey;
                confS3.Endpoint = benchmarkDetail.S3Endpoint;

                var resultS3 = await s3StorageService.Initialize(confS3);
                if (!resultS3.IsSuccess) return new KubeLifeResult<S3BenchmarkContainer>(false, resultS3.Message);

                return new KubeLifeResult<S3BenchmarkContainer>(true, "");
            }
            catch (Exception ex)
            {
                return new KubeLifeResult<S3BenchmarkContainer>(false, $"Exception on S3 connection : {ex.Message}");
            }
        }
    }
}
