using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement2_2.Models
{
    public class SqlEmployeeRepository : IEmployeeRepository
    {
        private AppDbContext _context;
        public SqlEmployeeRepository(AppDbContext context)
        {
            _context = context;
        }
        public Employee Create(Employee employee)
        {
            _context.Employees.Add(employee);
            _context.SaveChanges();
            return employee;
        }

        public Employee Delete(int id)
        {
            Employee employee = _context.Employees.Find(id);
            if(employee != null)
            {
                _context.Employees.Remove(employee);
                _context.SaveChanges();
            }
            return employee;
        }

        public Employee GetEmployee(int? id)
        {
            return _context.Employees.Find(id);
        }

        public List<Employee> GetEmployees()
        {
            return _context.Employees.ToList();
        }

        public Employee Update(Employee employeeChanges)
        {
            var employee = _context.Employees.Attach(employeeChanges);
            employee.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return employeeChanges;
        }
    }
}
