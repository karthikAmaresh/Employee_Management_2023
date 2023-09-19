using Application.Interfaces;
using Application.Queries;
using Domain.Entities;
using MediatR;

namespace Application.Handlers
{
    public class GetEmployeeListHandler : IRequestHandler<GetEmployeeListQuery, List<Employee>>
    {
        private readonly IEmployeeService _employeeService;

        public GetEmployeeListHandler(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public Task<List<Employee>> Handle(GetEmployeeListQuery request, CancellationToken cancellationToken)
        {
            return _employeeService.GetEmployees();
        }
    }
}
