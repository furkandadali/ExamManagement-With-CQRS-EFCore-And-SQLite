using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities.Models
{
    public class Log
    {
        public int Id { get; set; }
        public string Guid { get; set; }
        public string LogLevel { get; set; }
        public int? UserId { get; set; }
        public string PageUrl { get; set; }
        public string Parameters { get; set; }
        public string ExceptionMessage { get; set; }
        public string InnerException { get; set; }
        public string InInnerExceptionMessage { get; set; }
        public string RequestType { get; set; }
        public string StackTrace { get; set; }
        public string Method { get; set; }
        public string Action { get; set; }
        //[StringLength(64)]
        public string IpAddress { get; set; }
        public DateTime? CreatedDate { get; set; }

    }
    public class LogConfiguration : IEntityTypeConfiguration<Log>
    {
        public void Configure(EntityTypeBuilder<Log> builder)
        {

        }
    }
}
