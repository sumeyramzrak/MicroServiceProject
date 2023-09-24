using ESourcing.Order.Consumers;
using ESourcing.Order.Extensions;
using EventBusRabbitMQ;
using EventBusRabbitMQ.Producer;
using Microsoft.OpenApi.Models;
using Ordering.Application;
using Ordering.Infrastructure;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
#region AddInfrastructure
builder.Services.AddInfrastructure(builder.Configuration);
#endregion
#region AddApplication
builder.Services.AddApplication(); //ilgili ba��ml�l�klar� yazd���m�z method sayesinde projeye inject etmi� olduk.
#endregion
#region SwaggerDependencies
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Order API", Version = "v1" });
});
#endregion
#region EventBus
builder.Services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
{
    var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

    var factory = new ConnectionFactory()
    {
        HostName = builder.Configuration["EventBus:HostName"]
    };

    if (!string.IsNullOrWhiteSpace(builder.Configuration["EventBus:UserName"]))
    {
        factory.UserName = builder.Configuration["EventBus:UserName"];
    }

    if (!string.IsNullOrWhiteSpace(builder.Configuration["EventBus:Password"]))
    {
        factory.UserName = builder.Configuration["EventBus:Password"];
    }

    var retryCount = 5;
    if (!string.IsNullOrWhiteSpace(builder.Configuration["EventBus:RetryCount"]))
    {
        retryCount = int.Parse(builder.Configuration["EventBus:RetryCount"]);
    }

    return new DefaultRabbitMQPersistentConnection(factory, retryCount, logger);
    /*
    Uygulaman�n herhangi bir yerinden newleyerek ula�mak yerine asl�nda
    singleton lifecycle � ile dependency injection ile istedi�imiz konfigurasyonlarla olu�turmas�n� sa�l�yoruz.
    */
});
builder.Services.AddSingleton<EventBusOrderCreateConsumer>();
#endregion
#region AddAutoMapper
builder.Services.AddAutoMapper(typeof(Program));
//Program.cs �zerinde oldu�u assemblyi tan�mas� i�in ekledik.
#endregion
var app = builder.Build();
app.UseRabbitListener();
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
