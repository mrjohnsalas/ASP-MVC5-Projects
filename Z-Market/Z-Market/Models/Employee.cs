using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Z_Market.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public decimal Salary { get; set; }

        public decimal BonusPercent { get; set; }

        public DateTime DateOfBirth { get; set; }

        public DateTime StartTime { get; set; }

        public string Email { get; set; }

        public string Url { get; set; }

        public int DocumentTypeId { get; set; }

        public virtual DocumentType DocumentType { get; set; }
    }
}