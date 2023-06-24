using ESourcing.Sourcing.Data;
using ESourcing.Sourcing.Settings;
using Microsoft.Extensions.Options;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.Configure<SourcingDatabaseSettings>(builder.Configuration.GetSection(nameof(SourcingDatabaseSettings)));
builder.Services.AddSingleton<ISourcingDatabaseSettings>(sp =>sp.GetRequiredService<IOptions<SourcingDatabaseSettings>>().Value);

#region ProjectDependencies
builder.Services.AddTransient<ISourcingContext, SourcingContext>(); 
#endregion
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
