using CleanArchCQRSandMediator.API.Middleware;
using CleanArchCQRSandMediator.Application;
using CleanArchCQRSandMediator.Application.Common.Configurations;
using CleanArchCQRSandMediator.Application.Common.Interfaces;
using CleanArchCQRSandMediator.Domain.Entities.Identity;
using CleanArchCQRSandMediator.infra;
using CleanArchCQRSandMediator.infra.Authorization;
using CleanArchCQRSandMediator.infra.Data;
using CleanArchCQRSandMediator.infra.Persistence.InitialData;
using CleanArchCQRSandMediator.infra.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Globalization;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Location services
builder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "Resources";
});

// Supported languages
var supportedCultures = new[]
{
    new CultureInfo("es"),
    new CultureInfo("es-ES"),
    new CultureInfo("es-MX"),
    new CultureInfo("en"),
    new CultureInfo("en-US"),
    new CultureInfo("en-GB")
};

// Configure RequestLocalizationOptions
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("es");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;

    // Suppliers in order of priority
    options.RequestCultureProviders.Clear();
    options.RequestCultureProviders.Add(new QueryStringRequestCultureProvider
    {
        QueryStringKey = "lang"  // ?lang=es
    });
});

// Add services to the container.

// Load the configuration section and map it to the JwtSettings class
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
builder.Services.AddSingleton(jwtSettings!);

// Add layer dependency

// DbContext
builder.Services.AddInfrastructureServices(builder.Configuration);

// Identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

// Authentication JWT
var key = Encoding.UTF8.GetBytes(jwtSettings!.Secret);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtSettings.Audience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// Repositories and services
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

// Authorization handler registration (Scoped because it uses DbContext)
builder.Services.AddScoped<IAuthorizationHandler, PermissionHandler>();

// Custom Policy Provider Registration (Singleton)
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();

// Basic authorization
builder.Services.AddAuthorization();

// MediatR and FluentValidation (opcional pero recomendado)
builder.Services.AddApplicationServices();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200", "https://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

// Controllers with localization support in validations
builder.Services.AddControllers()
    .AddDataAnnotationsLocalization(); // For translations of validation messages

builder.Services.AddEndpointsApiExplorer();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingresar token JWT"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[] {}
        }
    });
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// builder.Services.AddOpenApi();

var app = builder.Build();

// Localization middleware (detects language based on request)
app.UseRequestLocalization();
app.UseMiddleware<LanguageMiddleware>();

// Data initialization (SEED)
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    try
    {
        if (serviceProvider == null) throw new ArgumentNullException();

        var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
        if (context == null) throw new InvalidOperationException();

        var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        await SeedData.InitializeAsync(context, roleManager, userManager);
    }
    catch (Exception ex)
    {
        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.MapOpenApi();

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngularApp");
app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
