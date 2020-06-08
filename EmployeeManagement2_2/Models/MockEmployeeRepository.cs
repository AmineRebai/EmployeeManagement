using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace EmployeeManagement2_2.Models
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        private List<Employee> _employeeList;
        public MockEmployeeRepository()
        {
            _employeeList = new List<Employee>()
            {
                new Employee{ Id = 1, Department = dept.IT, Email = "", Name = "Amine"},
                new Employee{ Id = 2, Department = dept.HR, Email = "", Name = "Ahmed"},
                new Employee{ Id = 3, Department = dept.Payroll, Email = "", Name = "Ali"},
            };
        }

        public Employee Create(Employee employee)
        {
            employee.Id = _employeeList.Max(x => x.Id) + 1;
            _employeeList.Add(employee);
            return employee;
        }

        public Employee Delete(int id)
        {
            Employee employee = _employeeList.FirstOrDefault(x => x.Id == id);
            if(employee != null)
            {
                _employeeList.Remove(_employeeList.First(x => x.Id == id));
            }
            return employee;
        }

        public Employee GetEmployee(int? id)
        {
            return _employeeList.Where(x => x.Id == id).FirstOrDefault();
        }

        public List<Employee> GetEmployees()
        {
            return _employeeList.ToList() ;
        }

        public Employee Update(Employee employeeChanges)
        {
            Employee employee = _employeeList.FirstOrDefault(x => x.Id == employeeChanges.Id);
            if (employee != null)
            {
                employee.Name = employeeChanges.Name;
                employee.Email = employeeChanges.Email;
                employee.Department = employeeChanges.Department;
            }
            return employee;
        }
        
    }
}
