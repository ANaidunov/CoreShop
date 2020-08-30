using CoreShop.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreShop.Data
{
    public class CoreShopContext : DbContext
    {
        public CoreShopContext(DbContextOptions<CoreShopContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
    }
}
