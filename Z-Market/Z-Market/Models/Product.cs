using System;
using System.ComponentModel.DataAnnotations;

namespace Z_Market.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public DateTime LastBuy { get; set; }

        public decimal Stock { get; set; }

        public string Remarks { get; set; }
    }
}