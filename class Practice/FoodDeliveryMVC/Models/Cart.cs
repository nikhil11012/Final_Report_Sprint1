using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDeliveryMVC.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Food")]
        public int FoodId { get; set; }
        public Food? Food { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
        public decimal TotalAmount { get; set; }
        public string? CustomerId { get; set; }
    }
}
