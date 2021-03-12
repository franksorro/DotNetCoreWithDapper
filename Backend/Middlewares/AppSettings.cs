namespace Backend.Middlewares
{
    public class AppSettings
    {
        /// <summary>
        /// 
        /// </summary>
        public AuthorizationSettings Authorization { get; set; }
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
}