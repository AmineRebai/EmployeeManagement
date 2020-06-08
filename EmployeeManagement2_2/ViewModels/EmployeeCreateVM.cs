using EmployeeManagement2_2.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement2_2.ViewModels
{
    public class EmployeeCreateVM
    {
        public int? Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-z-A-Z0-9-.]+$", ErrorMessage = "Invalid Email format")]
        public string Email { get; set; }
        [Required]
        public dept? Department { get; set; }
        public IFormFile Photo { get; set; }
    }
}
