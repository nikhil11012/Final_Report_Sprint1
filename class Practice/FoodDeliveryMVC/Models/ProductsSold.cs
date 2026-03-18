

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDeliveryMVC.Models
{
    public class ProductsSold
    {
        [Key]
        [Column(Order = 1)]
        [ForeignKey("Food")]
        public int ProductID { get; set; }
        [Key]
        [Column(Order = 1)]
        [ForeignKey("Sale")]
        public int SaleID { get; set; }
        public int Qty { get; set; }
        public decimal TotalAmount { get; set; }
        public string? status { get; set; }
        public Food? Food { get; set; }
        public Sale? Sale { get; set; }
    }
}
