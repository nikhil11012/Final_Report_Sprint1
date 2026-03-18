using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class AcademicStream
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        // Navigation properties
        public virtual ICollection<Student>? Students { get; set; }
        public virtual ICollection<Professor>? Professors { get; set; }
    }
}

