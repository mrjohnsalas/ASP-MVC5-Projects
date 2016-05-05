using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Z_Market.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "You must enter {0}")]
        [StringLength(30, ErrorMessage = "The field {0} must be between {2} and {1} characters", MinimumLength = 3)]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "You must enter {0}")]
        [StringLength(30, ErrorMessage = "The field {0} must be between {2} and {1} characters", MinimumLength = 3)]
        public string LastName { get; set; }

        [StringLength(9, ErrorMessage = "The field {0} must be between {2} and {1} characters", MinimumLength = 9)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "You must enter {0}")]
        [StringLength(30, ErrorMessage = "The field {0} must be between {2} and {1} characters", MinimumLength = 3)]
        public string Address { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Document")]
        [Required(ErrorMessage = "You must enter {0}")]
        [StringLength(20, ErrorMessage = "The field {0} must be between {2} and {1} characters", MinimumLength = 5)]
        public string Document { get; set; }

        [Required(ErrorMessage = "You must enter {0}")]
        public int DocumentTypeId { get; set; }

        [NotMapped]
        //public string FullName { get { return string.Format("{0} {1}", FirstName, LastName);}; set; }
        public string FullName { get { return string.Format("{0} {1}", FirstName, LastName); } }

        public virtual DocumentType DocumentType { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}