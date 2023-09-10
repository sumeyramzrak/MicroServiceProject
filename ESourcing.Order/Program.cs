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
builder.Services.AddApplication(); //ilgili bağımlılıkları yazdığımız method sayesinde projeye inject etmiş olduk.
#endregion
var app = builder.Build();
app.MigrateDatabase();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
