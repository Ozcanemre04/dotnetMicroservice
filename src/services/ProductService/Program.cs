using MassTransit;
using Microsoft.EntityFrameworkCore;
using ProductService.data;
using ProductService.services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddAutoMapper(typeof(Program));
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");;
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddMassTransit(config =>
                {
                    config.UsingRabbitMq((ctx, cfg) =>
                    {
                        var uri = new Uri(builder.Configuration["ServiceBus:Uri"]!);
                        cfg.Host(uri, h =>
                        {
                            h.Username(builder.Configuration["ServiceBus:Username"]);
                            h.Password(builder.Configuration["ServiceBus:Password"]);
                        });
                    });
});
builder.Services.AddMassTransitHostedService(true);
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();
var app = builder.Build();
DatabaseManagementService.MigrationInitialisation(app);

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
