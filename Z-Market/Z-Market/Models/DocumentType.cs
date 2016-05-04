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
        public int DocumentTypeId { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}