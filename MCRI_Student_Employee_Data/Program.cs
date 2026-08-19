using System.Text.Json.Serialization;
using MCRI_Student_Employee_Data.Data;
using MCRI_Student_Employee_Data.Repositories;
using MCRI_Student_Employee_Data.Services;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Database. Local development runs on SQL Server LocalDB, the deployed app runs on
// Supabase PostgreSQL. "Database:Provider" picks between them, so the only thing that
// changes between the two is configuration.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("ConnectionStrings:DefaultConnection is not configured.");

var provider = builder.Configuration["Database:Provider"] ?? "SqlServer";

builder.Services.AddDbContext<AppDbContext>(options =>
{
    if (provider.Equals("Postgres", StringComparison.OrdinalIgnoreCase))
    {
        options.UseNpgsql(connectionString);
    }
    else
    {
        options.UseSqlServer(connectionString);
    }
});

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

// Render terminates HTTPS at its edge and forwards over plain HTTP. Honouring the
// X-Forwarded-* headers keeps the app aware that the original request was HTTPS, so
// Swagger builds https:// URLs instead of http:// ones the browser then blocks.
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownIPNetworks.Clear();
    options.KnownProxies.Clear();
});

var app = builder.Build();

app.UseForwardedHeaders();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();

app.MapControllers();

// Opening the base URL lands on Swagger instead of a 404.
app.MapGet("/", () => Results.Redirect("/swagger"));

// Render pings this to decide whether the instance is live. It deliberately does not
// touch the database, so a database problem does not get the container restarted.
app.MapGet("/healthz", () => Results.Ok(new { status = "ok" }));

app.Run();
