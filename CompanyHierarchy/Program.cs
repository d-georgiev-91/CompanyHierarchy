using System.Text.Json.Serialization;
using CompanyHierarchy.Domain.Repositories;
using CompanyHierarchy.Infrastructure.Data;
using CompanyHierarchy.Infrastructure.Data.Repositories;
using CompanyHierarchy.Presentation.Mappings;
using CompanyHierarchy.Presentation.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(EmployeeProfile));

// Add services to the container.
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

builder.Services.AddSingleton<IDbConnectionFactory>(sp =>
    new NpgsqlConnectionFactory(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

builder.Services.AddValidatorsFromAssemblyContaining<EmployeeValidator>();
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddControllers().
    AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });
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