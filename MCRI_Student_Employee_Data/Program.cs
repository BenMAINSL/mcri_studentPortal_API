using System.Text.Json.Serialization;
using MCRI_Student_Employee_Data.Data;
using MCRI_Student_Employee_Data.Repositories;
using MCRI_Student_Employee_Data.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Database (local SQL Server for now; Supabase PostgreSQL for the workshop deployment)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Controllers -> Services -> Repositories
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IPersonRepository, PersonRepository>();


builder.Services.AddScoped<IImageStorageService, DatabaseImageStorageService>();


builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy => policy
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod()));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();

app.MapControllers();

// Opening the base URL lands on Swagger instead of a 404.
app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run();
