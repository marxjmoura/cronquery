using CronQuery.Mvc.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCronQuery(builder.Configuration.GetSection("CronQuery"));

builder.Services.AddTransient<MyFirstJob>();
builder.Services.AddTransient<MySecondJob>();
