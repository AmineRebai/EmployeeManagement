using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement2_2.ViewModels
{
    public class UserClaimsVM
    {
        public UserClaimsVM()
        {
            Cliams = new List<UserClaim>();
        }

        public string UserId { get; set; }
        public List<UserClaim> Cliams { get; set; }
    }
}
