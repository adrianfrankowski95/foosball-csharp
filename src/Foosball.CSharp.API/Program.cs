using Foosball.CSharp.API.Application.Queries;
using Foosball.CSharp.API.Extensions;
using Foosball.CSharp.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;
var env = builder.Environment;


services.AddControllers();
services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddInfrastructure(config)
    .AddQueriesAndCommandHandlers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
