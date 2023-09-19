using Application.Commands;
using Application.Interfaces;
using Azure;
using Domain.Entities;
using Domain.Entities.Context;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace Infrastructure.Services
{
    public class EmployeeService : IEmployeeService
    {
        private EmployeeDBContext _context;
        private ILogger<EmployeeService> _logger;
        private bool disposed = false;
        /* public EmployeeService(EmployeeDBContext context, ILogger<EmployeeService> logger)
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
     }*/

        public readonly Container _container;
        public EmployeeService(CosmosClient cosmosClient, string databaseName, string containerName)
        {
            _container = cosmosClient.GetContainer(databaseName,
            containerName);
        }

        public async Task<List<Employee>> GetEmployees()
        {
            try
            {
                var cosmosQuery = "Select * from c";
                var query = _container.GetItemQueryIterator<Employee>(new QueryDefinition(cosmosQuery));
                List<Employee> results = new List<Employee>();
                while (query.HasMoreResults)
                {
                    var response = await query.ReadNextAsync();
                    results.AddRange(response);
                }
                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR {ex.Message} Calling Get Employees");
                throw;
            }
        }
        public async Task<Employee> GetEmployee(string id)
        {
            try
            {
                var sql = "Select * from c where c.id = @id ";
                var cosmosQuery = new QueryDefinition(sql)
                .WithParameter("@id", id);

                var query = await _container.ReadItemAsync<Employee>(id.ToString(), new PartitionKey(id));
                return query.Resource;
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR {ex.Message} Calling Get Employee Data for employeeid ${id}");
                throw;
            }
        }

        public async Task<string> AddEmployee(AddEmployeeCommand employee)
        {
            try
            {
                string itemId = Guid.NewGuid().ToString();
                Employee newEmployee = new Employee()
                {
                    id = itemId,
                    about = employee.about,
                    address = employee.address,
                    age = employee.age,
                    company = employee.company,
                    email = employee.email,
                    eyeColor = employee.eyeColor,
                    firstName = employee.firstName,
                    lastName = employee.lastName,
                    phone = employee.phone,

                };
                
                ItemResponse<Employee> response = await _container.CreateItemAsync(newEmployee, new PartitionKey(newEmployee.id));
                var createdItem = response.Resource;
                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    return itemId; // Return the ID of the newly created item
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR {ex.Message} Calling Add Employee with parameters ${employee}");
                throw;
            }
        }
        public async Task<Employee> UpdateEmployee(UpdateEmployeeCommand employee)
        {
            try
            {
                var response = await _container.UpsertItemAsync(employee, new PartitionKey(employee.id));
                var resource = response.Resource;
                Employee updatedEmployeeDetails = new Employee()
                {
                    id = resource.id,
                    about = resource.about,
                    address = resource.address,
                    age = resource.age,
                    company = resource.company,
                    email = resource.email,
                    eyeColor = resource.eyeColor,
                    firstName = resource.firstName,
                    lastName = resource.lastName,
                    phone = resource.phone,
                };
                return updatedEmployeeDetails;
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR {ex.Message} Calling update Employee with parameters ${employee}");
                throw;
            }
        }

        public async Task<bool> DeleteEmployee(int id)
        {
            try
            {
                var response = await _container.DeleteItemAsync<Employee>(id.ToString(), new PartitionKey(id));
                if (response.IsBool())
                {
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

    }
}

