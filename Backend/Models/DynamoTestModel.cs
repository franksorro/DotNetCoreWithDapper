using Amazon.DynamoDBv2.DataModel;

namespace Backend.Models
{
    /// <summary>
    /// 
    /// </summary>
    [DynamoDBTable("test")]
    public class DynamoTestModel
    {
        /// <summary>
        /// 
        /// </summary>
        [DynamoDBHashKey("id")]
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Label { get; set; }
    }
}
