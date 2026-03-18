using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication21.Models
{
    public class Pharmacy
    {
        public int Id { get; set; }
        public string Name { set; get; }
        public DateTime EstablishedOn { set; get; }


    }
}
