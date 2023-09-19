using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Application.Validators
{
    /*public class ValidateEmployeeExistance : IActionFilter
    {
        private readonly IEmployeeService _employeeService;

        public ValidateEmployeeExistance(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            int id = 0;
            if (context.ActionArguments.ContainsKey("id"))
            {
                id = (int)context.ActionArguments["id"];
                if (id == 0)
                {
                    context.Result = new BadRequestResult();
                    return;
                }
            }
            else
            {
                context.Result = new BadRequestObjectResult("Bad id parameter");
                return;
            }
            *//*var employees = _employeeService.GetEmployees();
            var entity ;
            foreach(var emp in employees)
            if (entity == null)
            {
                context.Result = new NotFoundResult();
                return;
            }*//*
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }


        public static async Task<bool> CheckIfIdExistsAsync(List<int> domainIds, int targetId)
        {
            foreach (var id in domainIds)
            {
                if (id == targetId)
                {
                    return true;
                }
            }

            return false;
        }
    }*/
}
