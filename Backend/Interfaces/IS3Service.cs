using System.Threading.Tasks;
using Backend.Models;

namespace Backend.Interfaces
{
    public interface IS3Service
    {
        /// <summary>
        /// Create bucket
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        Task<S3Model> CreateBucket(string bucketName);

        /// <summary>
        /// Delete bucket
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        Task<S3Model> DeleteBucket(string bucketName);

        /// <summary>
        /// Upload file
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        Task<S3Model> Upload(string filePath, string bucketName);

        /// <summary>
        /// Get file content
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        Task<S3Model> Get(string bucketName, string fileName);

        /// <summary>
        /// Delete file
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        Task<S3Model> Delete(string bucketName, string fileName);
    }
}