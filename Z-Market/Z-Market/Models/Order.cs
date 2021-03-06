﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Z_Market.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        public DateTime DateOrder { get; set; }

        public int CustomerId { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public virtual Customer Customer { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}