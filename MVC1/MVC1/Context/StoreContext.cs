using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using MVC1.Models;

namespace MVC1.Context
{
    public class StoreContext:DbContext
    {
        public DbSet<Product> Products { get; set; }
    }
}