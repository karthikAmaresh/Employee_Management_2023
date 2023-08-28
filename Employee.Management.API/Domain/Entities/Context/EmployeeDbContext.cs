using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Context
{
    public class EmployeeDBContext : DbContext
    {
        public DbSet<Employee> Employee { get; set; }
        public EmployeeDBContext(DbContextOptions<EmployeeDBContext> options) : base(options) { }
    }
}
