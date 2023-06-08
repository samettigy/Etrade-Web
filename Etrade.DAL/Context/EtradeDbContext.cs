using Etrade.Entity.Models.Entities;
using Etrade.Entity.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etrade.DAL.Context
{
    public class EtradeDbContext : IdentityDbContext<User,Role,int>
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderLine> OrderLines { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=SAMETTIG39F5\SQLEXPRESS;Database=EtradeDb;Trusted_Connection=true; TrustServerCertificate=true");
        }
    }
}
