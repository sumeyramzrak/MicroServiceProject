using ESourcing.Order.Extensions;
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
var app = builder.Build();
app.MigrateDatabase();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
