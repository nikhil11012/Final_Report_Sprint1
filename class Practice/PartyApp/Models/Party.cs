using System.ComponentModel.DataAnnotations;

namespace PartyApp.Models
{
    public class Party
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public bool IsExternal { get; set; }
    }
}
