using ESourcing.Order.Extensions;
using Microsoft.OpenApi.Models;
using Ordering.Application;
using Ordering.Infrastructure;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
#region AddInfrastructure
builder.Services.AddInfrastructure(builder.Configuration);
#endregion
#region AddApplication
builder.Services.AddApplication(); //ilgili baðýmlýlýklarý yazdýðýmýz method sayesinde projeye inject etmiþ olduk.
#endregion
#region SwaggerDependencies
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Order API", Version = "v1" });
});
#endregion
var app = builder.Build();
app.MigrateDatabase();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Order API V1");
});
app.UseAuthorization();

app.MapControllers();

app.Run();
