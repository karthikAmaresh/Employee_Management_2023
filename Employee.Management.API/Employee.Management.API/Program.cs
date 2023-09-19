using MediatR;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Application.Handlers;
using Application.Interfaces;
using Application.Validators;
using Domain.Entities.Context;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Cosmos;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

/*builder.Services.AddScoped<ValidationFilter>();
*//*
builder.Services.AddDbContext<EmployeeDBContext>(options =>
    options.UseSqlServer(builder.Configuration["Data:DefaultConnection:ConnectionString"]).EnableSensitiveDataLogging());
*/
string url = builder.Configuration.GetSection("AzureCosmosDBSettings").GetValue<string>("URL");
string primaryKey = builder.Configuration.GetSection("AzureCosmosDBSettings").GetValue<string>("PrimaryKey");
string dbName = builder.Configuration.GetSection("AzureCosmosDBSettings").GetValue<string>("DatabaseName");
string containerName = builder.Configuration.GetSection("AzureCosmosDBSettings").GetValue<string>("ContainerName");


builder.Services.AddSingleton<IEmployeeService>(options =>
{

    var cosmosClient = new CosmosClient(url, primaryKey);
    return new EmployeeService(cosmosClient, dbName, containerName);

});

builder.Services.AddHttpClient("CustomHttpClient")
            .AddHttpMessageHandler<CustomDelegationHandler>();

builder.Services.Configure<ApiBehaviorOptions>(options
    => options.SuppressModelStateInvalidFilter = true);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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