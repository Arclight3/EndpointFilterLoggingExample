using Microsoft.AspNetCore.Http.HttpResults;
using TestEndpointFilters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/correctTypes", (ColorSelectorRequest colorSelectorRequest) =>
        new ColorSelectorResponse(colorSelectorRequest.Id, $"Your selected color: {colorSelectorRequest.Value}"))
    .AddRequestResponseLoggingFilter<ColorSelectorRequest, ColorSelectorResponse>();

app.MapPost("/wrongTypes", (ColorSelectorRequest colorSelectorRequest) =>
        new ColorSelectorResponse(colorSelectorRequest.Id, $"Your selected color: {colorSelectorRequest.Value}"))
    .AddRequestResponseLoggingFilter<ColorSelectorRequest2, ColorSelectorResponse2>();

app.MapPost("/requestOnly", (ColorSelectorRequest request) => { })
    .AddRequestLoggingFilter<ColorSelectorRequest>();

app.MapPost("/wrongRequestOnly", () => { })
    .AddRequestLoggingFilter<ColorSelectorRequest>();

app.MapPost("/wrongRequestOnly2", (ColorSelectorRequest2 request) => { })
    .AddRequestLoggingFilter<ColorSelectorRequest>();

app.MapPost("/responseOnly", () => new ColorSelectorResponse(1, "abc"))
    .AddResponseLoggingFilter<ColorSelectorResponse>();

app.MapPost("/wrongResponseOnly", () => { })
    .AddResponseLoggingFilter<ColorSelectorResponse>();

app.MapPost("/wrongResponseOnly2", () => new ColorSelectorResponse(1, "abc"))
    .AddResponseLoggingFilter<ColorSelectorResponse2>();

app.MapPost("/wrongResponseOnly3", () => (ColorSelectorResponse)null)
    .AddResponseLoggingFilter<ColorSelectorResponse2>();

app.Run();

record ColorSelectorRequest(int Id, string Value);
record ColorSelectorRequest2(int Id, string Value);
record ColorSelectorResponse(int Id, string Message);
record ColorSelectorResponse2(int Id, string Message);