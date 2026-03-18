using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [ForeignKey("AcademicStream")]
        public int StreamId { get; set; }

        [Required]
        [ForeignKey("Parent")]
        public int ParentId { get; set; }

        // Navigation properties
        public virtual AcademicStream? AcademicStream { get; set; }
        public virtual Parent? Parent { get; set; }
    }
}
