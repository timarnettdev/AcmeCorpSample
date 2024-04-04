using AcmeCore.Service.Services.Interfaces;
using AcmeCore.Service.Services;
using Microsoft.EntityFrameworkCore;
using AcmeCorp.Infrastructure.Repositories;
using AcmeCorp.Domain;
using AcmeCorp.Infrastructure.Automapper;
using AcmeCorp.API;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });
    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.ApiKey,
        Name = "ApiKey",
        In = ParameterLocation.Header,
        Description = "API Key Authentication"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            new string[] {}
        }
    });
});
builder.Services.AddAutoMapper(typeof(MappingProfile));
var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<AcmeCorpDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped<AcmeCorpDbContext, AcmeCorpDbContext>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICustomerService, CustomerService>();

var app = builder.Build();
app.UseApiKeyMiddleware();
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
