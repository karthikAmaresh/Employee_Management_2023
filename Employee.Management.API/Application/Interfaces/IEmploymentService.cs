using Application.Commands;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IEmployeeService
    {
        public List<Employee> GetEmployees();
        public Employee GetEmployee(int id);
        public Employee AddEmployee(AddEmployeeCommand employee);
        public Employee UpdateEmployee(UpdateEmployeeCommand employee);
        public bool DeleteEmployee(int id);

    }
}
