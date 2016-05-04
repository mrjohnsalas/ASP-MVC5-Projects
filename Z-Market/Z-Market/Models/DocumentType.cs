using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Z_Market.Models
{
    public class DocumentType
    {
        [Key]
        [Display(Name = "Document Type")]
        public int DocumentTypeId { get; set; }

        [Display(Name = "Document")]
        public string Description { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
    }
}