using Azure;
using Azure.Data.Tables;

namespace Prog_POE.Models
{
    public class User : ITableEntity
    {
        public string PartitionKey { get; set; } = "Lecture";
        public string RowKey { get; set; } 
        public string Username { get; set; }
        public string Password { get; set; } 
        public int Id { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
