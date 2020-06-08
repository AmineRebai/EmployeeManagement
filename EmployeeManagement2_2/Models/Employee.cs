using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement2_2.Models
{
    public class Employee
    {
        public int? Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-z-A-Z0-9-.]+$", ErrorMessage ="Invalid Email format")]
        public string Email { get; set; }
        [Required]
        public dept? Department { get; set; }
        public string PhotoPath { get; set; }
    }
}
