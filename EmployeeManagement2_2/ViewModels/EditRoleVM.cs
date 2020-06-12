using EmployeeManagement2_2.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement2_2.ViewModels
{
    public class EditRoleVM
    {
        public EditRoleVM()
        {
            Users = new List<string>();
        }
        public string Id { get; set; }
        public string RoleName { get; set; }
        public List<string> Users { get; set; }
    }
}
