using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using WebApplication21.Models;

namespace DAL
{
    public class PharmacyContext:DbContext
    {
        public PharmacyContext(DbContextOptions<PharmacyContext> options) : base(options)
        {
        }

        public DbSet<Pharmacy>Pharmacies { get; set; }
        public DbSet<Medicine> Medicines { get; set; }

    }
}
