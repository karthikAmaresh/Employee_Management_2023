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
        public Task<List<Employee>> GetEmployees();
        public Task<Employee> GetEmployee(string id);
        public Task<string> AddEmployee(AddEmployeeCommand employee);
        public Task<Employee> UpdateEmployee(UpdateEmployeeCommand employee);
        public Task<bool> DeleteEmployee(int id);

    }
}
