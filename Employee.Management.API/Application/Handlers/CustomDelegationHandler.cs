using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers
{
    public class CustomDelegationHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Perform any pre-processing logic before sending the request
            // For example, you can add headers, modify the request, or log information

            // Call the inner handler to continue the request processing
            var response = await base.SendAsync(request, cancellationToken);

            // Perform any post-processing logic on the response
            // For example, you can modify the response, log information, or handle errors

            return response;
        }
    }
}
