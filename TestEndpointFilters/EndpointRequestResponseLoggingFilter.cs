using Microsoft.AspNetCore.Http.HttpResults;
using System.Text.Json;

namespace TestEndpointFilters;

public class EndpointRequestResponseLoggingFilter<TRequest, TResponse> : IEndpointFilter
{
    private readonly ILogger _logger;

    public EndpointRequestResponseLoggingFilter(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<EndpointRequestResponseLoggingFilter<TRequest, TResponse>>();
    }

    public virtual async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        // Log request
        if (context.Arguments.Count > 0)
        {
            var request = context.Arguments[0];

            if (request is TRequest)
                _logger.LogInformation("Request: {request}", JsonSerializer.Serialize(request));
            else if (request is null)
                _logger.LogError("Request: null");
            else
            {
                _logger.LogError("Expected request type '{providedRequestType}' but found type '{actualRequestType}'.", typeof(TRequest).FullName, request.GetType().FullName);
                _logger.LogInformation("Request: {request}", JsonSerializer.Serialize(request));
            }
        }
        else if (typeof(TRequest) != typeof(EmptyHttpRequest))
            _logger.LogError("Expected request type '{providedRequestType}' but no request object was received.", typeof(TRequest).FullName);
        else
            _logger.LogInformation("Request: No request");

        var response = await next(context);

        // Log response
        if (response is TResponse)
            _logger.LogInformation("Response: {response}", JsonSerializer.Serialize(response));
        else
        {
            if (response is null)
                _logger.LogError("Response: null");
            else
            {
                if (response.GetType().Name is nameof(EmptyHttpResult))
                {
                    if (typeof(TResponse) != typeof(EmptyHttpResult))
                        _logger.LogError("Expected response type '{providedResponseType}' but no response was returned.", typeof(TResponse).FullName);
                    else
                        _logger.LogInformation("Response: No response");
                }
                else
                {
                    _logger.LogError("Expected response type '{providedResponseType}' but found type '{actualResponseType}'.", typeof(TResponse).FullName, response.GetType().FullName);
                    _logger.LogInformation("Response: {response}", JsonSerializer.Serialize(response));
                }
            }
        }

        return response;
    }
}