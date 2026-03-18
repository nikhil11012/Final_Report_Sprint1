using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    public class Professor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [ForeignKey("AcademicStream")]
        public int StreamId { get; set; }

        [StringLength(100)]
        [EmailAddress]
        public string? Email { get; set; }

        [StringLength(20)]
        [Phone]
        public string? PhoneNumber { get; set; }

        // Navigation properties
        public virtual AcademicStream? AcademicStream { get; set; }
    }
}
