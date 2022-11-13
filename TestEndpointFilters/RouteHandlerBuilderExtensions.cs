using Microsoft.AspNetCore.Http.HttpResults;

namespace TestEndpointFilters;

public static class RouteHandlerBuilderExtensions
{
    public static RouteHandlerBuilder AddRequestResponseLoggingFilter<TRequest, TResponse>(this RouteHandlerBuilder builder)
    {
        builder.AddEndpointFilter<EndpointRequestResponseLoggingFilter<TRequest, TResponse>>();
        return builder;
    }

    public static RouteHandlerBuilder AddRequestLoggingFilter<TRequest>(this RouteHandlerBuilder builder)
    {
        builder.AddEndpointFilter<EndpointRequestResponseLoggingFilter<TRequest, EmptyHttpResult>>();
        return builder;
    }

    public static RouteHandlerBuilder AddResponseLoggingFilter<TResponse>(this RouteHandlerBuilder builder)
    {
        builder.AddEndpointFilter<EndpointRequestResponseLoggingFilter<EmptyHttpRequest, TResponse>>();
        return builder;
    }
}