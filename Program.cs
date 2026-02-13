using System.Text;
using ELTBackend.Data;
using ELTBackend.Mappings;
using ELTBackend.Repositories;
using ELTBackend.Services;
using ELTBackend.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MovieHub.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
#region Services
// Register controllers
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddControllers();

// Register DbContext with SQL Server
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString))
{
    connectionString = builder.Configuration["DB_CONNECTION"];
}
builder.Services.AddDbContext<EmployeeLeaveTrackerDbContext>(options =>
    options.UseNpgsql(connectionString)
);

// Regsiter swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Enter the JWT token.",
        }
    );

    c.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer",
                    },
                },
                Array.Empty<string>()
            },
        }
    );
});

// Regster CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

// Register Authentication
builder
    .Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        var config = builder.Configuration;
        var key =
            config["JwtSettings:Key"]
            ?? throw new InvalidOperationException("Jwt Key is missing in configuration!");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
        };
    });

// Register Authorization
builder.Services.AddAuthorization();

// Register all the service classes
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserLeaveService, UserLeaveService>();

// Register all the repository classes
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ILeaveRepository, LeaveRepository>();

// Register all the utility classes
builder.Services.AddScoped<IJwtHelper, JwtHelper>();
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
#region Middlewares
// Enable Swagger for development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
#endregion

app.MapControllers();

app.Run();
