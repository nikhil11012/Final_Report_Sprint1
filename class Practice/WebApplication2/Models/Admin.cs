using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Admin
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(100)]
        [EmailAddress]
        public string? Email { get; set; }

        [StringLength(20)]
        [Phone]
        public string? PhoneNumber { get; set; }
    }
}
