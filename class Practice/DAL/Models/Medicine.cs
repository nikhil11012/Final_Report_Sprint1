using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication21.Models;

namespace DAL.Models
{
        public class Medicine
        {
            public int Id { get; set; }
            public string Name { set; get; }
            [ForeignKey("Pharma")]
            public int PharmacyId { get; set; }
            public Pharmacy? Pharma { get; set; }
            public int Qty { set; get; }
        }
}
