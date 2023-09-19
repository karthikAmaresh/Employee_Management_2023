using Application.Commands;
using Application.Queries;
using Application.Validators;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Employee.Management.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(IMediator mediator, ILogger<EmployeeController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        // GET: api/<EmployeeController>
        [HttpGet]
        public async Task<List<Domain.Entities.Employee>> GetEmployees()
        {
            return await _mediator.Send(new GetEmployeeListQuery());

        }

        // GET api/<EmployeeController>/5
        [HttpGet("{id}")]
/*        [ServiceFilter(typeof(ValidateEmployeeExistance))]
*/
        public async Task<IActionResult> GetEmployeeById(string id)
        {
            var result = _mediator.Send(new GetEmployeeByIdQuery(id));
            return new OkObjectResult(result);
        }

        // POST api/<EmployeeController>
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilter))]
        public async Task<IActionResult> AddEmployee([FromBody] AddEmployeeCommand command)
        {
            var validate = new EmployeeEmailVaidator();
            var validationResult = validate.Validate(command);
            if (validationResult.IsValid)
            {
                try
                {
                    if (command.company != null
                    && command.email != null)
                    {
                        var address = new System.Net.Mail.MailAddress(command.email);
                        var emailDomain = address.Host;

                        var isDomainExists = emailDomain.Equals(command.company + ".com", StringComparison.OrdinalIgnoreCase);
                        if (!isDomainExists)
                        {
                            ModelState.AddModelError(nameof(Domain.Entities.Employee.email), "please provide email provided by company domain!");
                        }

                    }
                    if (!ModelState.IsValid)
                    {
                        return UnprocessableEntity(ModelState);
                    }
                    var result = _mediator.Send(command);
                    return new OkObjectResult(result);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Exception caught while adding employee: {ex.Message}");
                    return new BadRequestResult();
                }
            }
            else
            {
                return new BadRequestObjectResult(validationResult.Errors.Select(x => x.ErrorMessage));
            }
        }

        // PUT api/<EmployeeController>/5
        /*[HttpPut]
        public async Task<IActionResult> UpdateEmployee(UpdateEmployeeCommand command)
        {

            var validate = new UpdateEmployeeEmailVaidator();
            var validationResult = validate.Validate(command);
            if (validationResult.IsValid)
            {
                try
                {
                    if (command.company != null
                    && command.email != null)
                    {
                        var address = new System.Net.Mail.MailAddress(command.email);
                        var emailDomain = address.Host;

                        var isDomainExists = emailDomain.Equals(command.company + ".com", StringComparison.OrdinalIgnoreCase);
                        if (!isDomainExists)
                        {
                            ModelState.AddModelError(nameof(Domain.Entities.Employee.email), "please provide email provided by company domain!");
                        }

                    }
                    if (!ModelState.IsValid)
                    {
                        return UnprocessableEntity(ModelState);
                    }
                    var result = _mediator.Send(command);
                    return new OkObjectResult(result);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Exception caught while Updating employee : {ex.Message}");
                    return new BadRequestResult();
                }
            }
            else
            {
                return new BadRequestObjectResult(validationResult.Errors.Select(x => x.ErrorMessage));
            }
        }*/

        // DELETE api/<EmployeeController>/5
        [HttpDelete("{id}")]
        /*[ServiceFilter(typeof(ValidateEmployeeExistance))]*/

        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                var result = _mediator.Send(new DeleteEmployeeById(id));
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception caught while removing employee: {ex.Message}");
                return new BadRequestResult();
            }
        }
    }
}
