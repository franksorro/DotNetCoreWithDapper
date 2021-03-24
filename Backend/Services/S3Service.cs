using System.Threading.Tasks;
using Amazon.S3;
using System.Net;
using Amazon.S3.Model;
using Amazon.S3.Util;
using System.IO;
using Amazon.S3.Transfer;
using Backend.Interfaces;
using Backend.Models;

namespace Backend.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class S3Service : IS3Service
    {

        private readonly IAmazonS3 s3;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s3"></param>
        public S3Service(IAmazonS3 s3)
        {
            this.s3 = s3;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        public async Task<S3Model> CreateBucket(string bucketName)
        {
            try
            {
                if (await AmazonS3Util.DoesS3BucketExistV2Async(s3, bucketName))
                {
                    return new S3Model()
                    {
                        Message = $"Bucket {bucketName} exists",
                        Status = HttpStatusCode.BadRequest
                    };
                };

                PutBucketRequest putBucketRequest = new PutBucketRequest
                {
                    BucketName = bucketName,
                    UseClientRegion = true
                };

                PutBucketResponse response = await s3.PutBucketAsync(putBucketRequest);

                return new S3Model
                {
                    Message = response.ResponseMetadata.RequestId,
                    Status = response.HttpStatusCode
                };
            }
            catch (AmazonS3Exception e)
            {
                return new S3Model
                {
                    Message = e.Message,
                    Status = e.StatusCode
                };
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        public async Task<S3Model> DeleteBucket(string bucketName)
        {
            try
            {
                if (!await AmazonS3Util.DoesS3BucketExistV2Async(s3, bucketName))
                {
                    return new S3Model()
                    {
                        Message = $"Bucket {bucketName} not exists",
                        Status = HttpStatusCode.BadRequest
                    };
                };

                DeleteBucketResponse response = await s3.DeleteBucketAsync(bucketName);

                return new S3Model
                {
                    Message = response.ResponseMetadata.RequestId,
                    Status = response.HttpStatusCode
                };
            }
            catch (AmazonS3Exception e)
            {
                return new S3Model
                {
                    Message = e.Message,
                    Status = e.StatusCode
                };
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task<S3Model> Delete(string bucketName, string fileName)
        {
            try
            {
                GetObjectRequest request = new GetObjectRequest
                {
                    BucketName = bucketName,
                    Key = fileName
                };

                DeleteObjectResponse response = await s3.DeleteObjectAsync(bucketName, fileName);

                return new S3Model
                {
                    Message = response.ResponseMetadata.RequestId,
                    Status = response.HttpStatusCode
                };
            }
            catch (AmazonS3Exception e)
            {
                return new S3Model
                {
                    Message = e.Message,
                    Status = e.StatusCode
                };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task<S3Model> Get(string bucketName, string fileName)
        {
            try
            {
                GetObjectRequest request = new GetObjectRequest
                {
                    BucketName = bucketName,
                    Key = fileName
                };

                string contentData;

                using (var response = await s3.GetObjectAsync(request))
                using (var responseStream = response.ResponseStream)
                using (var reader = new StreamReader(responseStream))
                {
                    contentData = reader.ReadToEnd();
                };

                return new S3Model
                {
                    Message = "Success",
                    Status = HttpStatusCode.OK,
                    Data = contentData
                };
            }
            catch (AmazonS3Exception e)
            {
                return new S3Model
                {
                    Message = e.Message,
                    Status = e.StatusCode
                };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        public async Task<S3Model> Upload(string filePath, string bucketName)
        {
            try
            {
                var fileTransferUtility = new TransferUtility(s3);

                await fileTransferUtility.UploadAsync(filePath, bucketName);

                return new S3Model
                {
                    Message = "File uploaded Successfully",
                    Status = HttpStatusCode.OK
                };
            }
            catch (AmazonS3Exception e)
            {
                return new S3Model
                {
                    Message = e.Message,
                    Status = e.StatusCode
                };
            };
        }
    }
}