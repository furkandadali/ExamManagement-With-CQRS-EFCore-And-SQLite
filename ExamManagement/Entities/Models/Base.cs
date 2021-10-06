using System;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public abstract class Base
    {
        [Key]
        [Required]
        public int Id { get; set; }
        //public int UserId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? ModifiedDate { get; set; }
        //public int? CreatedExamId { get; set; }
        //public int? ModifiedExamId { get; set; }
    }
}
