using System.Reflection;
using Microsoft.OpenApi.Models;
using ParkingManagement.Api.Endpoints;
using ParkingManagement.Api.Extensions;
using ParkingManagement.Api.Middlewares;
using ParkingManagement.Application;
using ParkingManagement.Infrastructure;
using ParkingManagement.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Configure JSON Options for Minimal APIs
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
});

// 2. Configure Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Parking Management Web API",
        Version = "v1",
        Description = "Smart parking management API following Domain-Driven Design (DDD) and Clean Architecture.",
        Contact = new OpenApiContact
        {
            Name = "Parking System Support Team",
            Email = "support@parkingmanagement.com"
        }
    });

    // Configure Swagger Bearer Authentication
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Enter JWT Bearer token format: Bearer {your token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// 3. Register Architecture Layers
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddApplicationServices();
builder.Services.AddEndpoints(typeof(Program).Assembly);

// 3.1 Configure Authentication & JWT Bearer
var jwtKey = builder.Configuration["Jwt:Key"] ?? "SuperSecretKeyForParkingSystem_1234567890!@#$%^&*()";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "ParkingManagement_Api";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "ParkingManagement_Client";

builder.Services.AddAuthentication(Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

// 4. Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// 5. Global Exception Middleware
app.UseMiddleware<GlobalExceptionMiddleware>();

// 6. Swagger UI configuration
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Parking Management API v1");
    c.RoutePrefix = string.Empty;
});

app.UseCors("AllowAll");
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.UseAuthentication();
app.UseAuthorization();
app.MapEndpoints();

// 7. Initialize Database and Seed Data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    try
    {
        var dbContext = services.GetRequiredService<ParkingDbContext>();
        dbContext.Database.EnsureCreated();
        await DataSeeder.SeedAsync(dbContext);
        logger.LogInformation("Database is initialized and sample data seeded successfully.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred during database initialization.");
    }
}

app.Run();
