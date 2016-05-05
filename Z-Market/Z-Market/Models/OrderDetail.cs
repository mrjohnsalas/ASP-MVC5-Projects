using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Z_Market.Models
{
    public class OrderDetail
    {
        [Key]
        public int OrderDetailId { get; set; }

        public int OrderId { get; set; }

        public int ProductId { get; set; }

        [Display(Name = "Product Description")]
        [Required(ErrorMessage = "You must enter {0}")]
        [StringLength(30, ErrorMessage = "The field {0} must be between {2} and {1} characters", MinimumLength = 3)]
        public string Description { get; set; }

        [Required(ErrorMessage = "You must enter {0}")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "You must enter {0}")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        [DataType(DataType.Currency)]
        public decimal Quantity { get; set; }

        public virtual Order Order { get; set; }

        public virtual Product Product { get; set; }
    }
}