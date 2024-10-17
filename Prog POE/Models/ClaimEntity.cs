using Azure;
using Azure.Data.Tables;
using System.ComponentModel.DataAnnotations;

namespace Prog_POE.Models
{
    public class Claims : ITableEntity
    {
        [Key]
        public string? PartitionKey { get; set; }
        public string? RowKey { get; set; }
        public DateTime Timestamp { get; set; }
        public string? LectureName { get; set; }
        public string? LectureID { get; set; }
        public int? HoursWorked { get; set; }
        public int? HourlyRate { get; set; }
        public string? ClaimDetails { get; set; }
        public string? AdditionalNotes { get; set; }
        public string? ClaimDescription { get; set; }
        public ETag ETag { get; set; }

    }
}