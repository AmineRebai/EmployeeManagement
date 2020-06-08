using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement2_2.Models
{
    public interface IEmployeeRepository
    {
        Employee GetEmployee(int? id);
        List<Employee> GetEmployees();
        Employee Create(Employee employee);
        Employee Update(Employee employeeChanges);
        Employee Delete(int id);
    }
}
