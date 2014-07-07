using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using OnBalance.Domain.Entities;

namespace OnBalance.Domain.Concrete
{
    public class EfDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Organization> Organizations { get; set; }

        /// <summary>
        /// Balance items for syncing between OBS and POS
        /// </summary>
        public DbSet<BalanceItem> BalanceItems { get; set; }

        public DbSet<ProductDetail> ProductDetails { get; set; }

        //public EfDbContext(string connectionString)
        //    : base(connectionString)
        //{

        //}
    }
}
