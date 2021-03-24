namespace Backend.Middlewares
{
    /// <summary>
    /// 
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// 
        /// </summary>
        public AuthorizationSettings Authorization { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public AWSSettings AWS { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AuthorizationSettings
    {
        /// <summary>
        /// 
        /// </summary>
        public string ApiKeyName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ApiKeyValue { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AWSSettings
    {
        /// <summary>
        /// 
        /// </summary>
        public string AccessKeyId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AWSSecretKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Region { get; set; }
    }
}