using Application.Commands;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class ValidateEmployeeOnUpdate : IActionFilter
    {
        private readonly IEmployeeService _employeeService;

        public ValidateEmployeeOnUpdate(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var command = context.ActionArguments.Values.OfType<UpdateEmployeeCommand>().FirstOrDefault();

            if (command.id == null)
            {
                context.Result = new BadRequestResult();
                return;
            }
            var employee = _employeeService.GetEmployee(command.id);
            if (employee == null)
            {
                context.Result = new NotFoundResult();
                return;
            }
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
