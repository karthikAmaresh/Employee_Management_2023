using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries
{
    public class DeleteEmployeeById : IRequest<bool>
    {
        public int EmployeeId { get; set; }

        public DeleteEmployeeById(int employeeId)
        {
            EmployeeId = employeeId;
        }
    } 
}
