using System.Net;

namespace Core.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class S3Model
    {
        /// <summary>
        /// 
        /// </summary>
        public HttpStatusCode Status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public object Data { get; set; }
    }
}