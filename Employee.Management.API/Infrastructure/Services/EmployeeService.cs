using Application.Commands;
using Application.Interfaces;
using Domain.Entities;
using Domain.Entities.Context;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class EmployeeService : IEmployeeService, IDisposable
    {
        private EmployeeDBContext _context;
        private ILogger<EmployeeService> _logger;
        private bool disposed = false;
        public EmployeeService(EmployeeDBContext context, ILogger<EmployeeService> logger)
        {
            _context = context;
            _logger = logger;
        }
        public List<Employee> GetEmployees()
        {
            try
            {
                return _context.Employee.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR {ex.Message} Calling Get Employees");
                throw;
            }
        }
        public Employee GetEmployee(int id)
        {
            try
            {
                return _context.Employee.FirstOrDefault(x => x.id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR {ex.Message} Calling Get Employee Data for employeeid ${id}");
                throw;
            }
        }

        public Employee AddEmployee(AddEmployeeCommand employee)
        {
            try
            {
                Employee newEmployee = new Employee()
                {
                    eyeColor = employee.eyeColor,
                    about = employee.about,
                    address = employee.address,
                    age = employee.age,
                    company = employee.company,
                    email = employee.email,
                    firstName = employee.firstName,
                    lastName = employee.lastName,
                    phone = employee.phone
                };
                _context.Employee.Add(newEmployee);
                _context.SaveChanges();
                return newEmployee;
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR {ex.Message} Calling Add Employee with parameters ${employee}");
                throw;
            }
        }
        public Employee UpdateEmployee(UpdateEmployeeCommand employee)
        {
            try
            {
                Employee employeeData = GetEmployee(employee.id);
                if (employeeData != null)
                {
                    Employee employeeDetails = new()
                    {
                        id = employee.id,
                        about = employee.about,
                        firstName = employee.firstName,
                        lastName = employee.lastName,
                        eyeColor = employee.eyeColor,
                        company = employee.company,
                        phone = employee.phone,
                        email = employee.email,
                        address = employee.address,
                        age = employee.age
                    };
                    _context.Employee.Update(employeeDetails);
                    _context.SaveChanges();
                    return employeeDetails;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR {ex.Message} Calling update Employee with parameters ${employee}");
                throw;
            }
        }

        public bool DeleteEmployee(int id)
        {
            try
            {
                Employee employee = GetEmployee(id);
                if (employee != null)
                {
                    _context.Employee.Remove(employee);
                    _context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR {ex.Message} Calling Delete Employee with employee id ${id}");
                throw;
            }

        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
