using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Z_Market.Models
{
    public class ProductOrder : Product
    {
        [Required(ErrorMessage = "You must enter {0}")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal Quantity { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal Total { get { return Price * Quantity; } }
    }
}