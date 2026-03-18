using System.ComponentModel.DataAnnotations.Schema;

namespace DOTNETCOREWEBAPI.Models
{
    public class Employees
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [ForeignKey("dept")]
        public int DeptId { get; set; }
        public Department dept { get; set; }
    }
}
