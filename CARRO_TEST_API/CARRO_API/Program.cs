using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Text;
using Microsoft.OpenApi.Models;
using CARRO_API.Middlewares;
using CARRO_API.Models;
using Microsoft.EntityFrameworkCore;
using CARRO_API.Entities;
using CARRO_API.Services.Interface;
using CARRO_API.Services;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var commandTimeout = builder.Configuration["CommandTimeout"];
int timeoutValue = 30;
if (commandTimeout != null)
    timeoutValue = Convert.ToInt32(commandTimeout);

// Load configuration from appsettings.json
var appSettings = builder.Configuration.GetSection("AppSettings").Get<AppSettings>();
builder.Services.AddSingleton(appSettings);

var envVar = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{envVar}.json", true, true);

builder.Services.AddDbContext<DbDataContext>(options =>
              options.UseSqlServer(builder.Configuration.GetConnectionString("DbDataContext"), opt => opt.CommandTimeout(timeoutValue)));

// Add a scoped service
builder.Services.AddScoped<IAuthrorizationsService, AuthrorizationsService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEmailService, EmailService>();

// Add services to the container.
builder.Services.AddControllers();

// Configure JWT settings using appsettings.json
var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
builder.Services.AddSingleton(jwtSettings);

// Configure Middleware settings using appsettings.json
var middleWaresSettings = configuration.GetSection("MiddlewareSettings").Get<MiddlewareSettings>();
builder.Services.AddSingleton(jwtSettings);

// Configure SMTP settings using appsettings.json
var smtpSettings = configuration.GetSection("SmtpSettings").Get<SmtpSettings>();
builder.Services.AddSingleton(smtpSettings);

// Configure Swagger using appsettings.json
var swaggerSettings = configuration.GetSection("SwaggerSettings").Get<SwaggerSettings>();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc(swaggerSettings.Version, new OpenApiInfo { 
        Title = swaggerSettings.Title + ", ENV : " + (appSettings?.Environment ?? "-"), 
        Version = swaggerSettings.Version,
        Contact = new OpenApiContact
        {
            Name = swaggerSettings.Contact.Name,
            Url = new Uri(swaggerSettings.Contact.Url),
            Email = swaggerSettings.Contact.Email
        },
    });
    c.IgnoreObsoleteProperties();
    var securitySchema = new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header \"Authorization: Bearer {token}\"",
        Name = "authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer",
        }
    };
    c.AddSecurityDefinition("Bearer", securitySchema);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { securitySchema, new[] { "Bearer" } },
                });
    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "ApiKey must appear in header",
        Type = SecuritySchemeType.ApiKey,
        Name = "XApiKey",
        In = ParameterLocation.Header,
        Scheme = "ApiKeyScheme"
    });
    var key = new OpenApiSecurityScheme()
    {
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "ApiKey"
        },
        In = ParameterLocation.Header
    };
    var requirement = new OpenApiSecurityRequirement
                    {
                             { key, new List<string>() }
                    };
    c.AddSecurityRequirement(requirement);
});

builder.Services.AddCors(o => o.AddPolicy("CarrotPolicy", builder =>
{
    //builder.WithOrigins(appSettings.AllowedHosts)
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
}));

// Add JWT token authentication
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

    })
    .AddJwtBearer(cfg =>
    {
        cfg.RequireHttpsMetadata = false;
        cfg.SaveToken = true;
        cfg.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
            ClockSkew = TimeSpan.Zero // remove delay of token when expire
        };
    });


var app = builder.Build();
IWebHostEnvironment env = app.Environment;

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint(swaggerSettings.Endpoint, swaggerSettings.Title);
        c.RoutePrefix = swaggerSettings.RoutePrefix;
        c.DocExpansion(DocExpansion.List);
    });
    //app.UseSwaggerUI();
}

app.UseCors("CarrotPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

// app.UseMiddleware<ApiKeyAuthenticationMiddleware>();

app.MapControllers();

app.Run();
