using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement2_2.ViewModels
{
    public class EmployeeEditViewModel : EmployeeCreateVM
    {
        public string ExistingPhotoPath { get; set; }
    }
}
