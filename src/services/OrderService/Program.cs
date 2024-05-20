using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderService;
using OrderService.data;
using OrderService.services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(typeof(Program));
var server = builder.Configuration["server"];
var port = builder.Configuration["port"];
var database = builder.Configuration["database"];
var user = builder.Configuration["user"];
var password = builder.Configuration["password"];
var connectionString = $"Server={server}, {port};Database={database};User ID={user};Password={password};TrustServerCertificate=True";
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddMassTransit(config =>
                {
                    config.AddConsumer<ProductConsumer>();
                    config.AddConsumer<ProductDeletedConsumer>();
                    config.UsingRabbitMq((ctx, cfg) =>
                    {
                        var uri = new Uri(builder.Configuration["ServiceBus:Uri"]!);
                        cfg.Host(uri, h =>
                        {
                            h.Username(builder.Configuration["ServiceBus:Username"]);
                            h.Password(builder.Configuration["ServiceBus:Password"]);
                        });
                    
                    cfg.ReceiveEndpoint(builder.Configuration["ServiceBus:Queue"],c=>
                    {
                          c.ConfigureConsumer<ProductConsumer>(ctx);
                          c.ConfigureConsumer<ProductDeletedConsumer>(ctx);
                          
                    });
                    });
                
});
builder.Services.AddMassTransitHostedService(true);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
