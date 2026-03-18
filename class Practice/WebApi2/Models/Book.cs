using System.Data;

namespace WebApi2.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { set; get; }
        public string Description { set; get; }
        public DateTime? PublishDate { get; set; } = DateTime.Now;

    }
}
