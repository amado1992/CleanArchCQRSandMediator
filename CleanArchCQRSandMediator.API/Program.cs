using CleanArchCQRSandMediator.Application;
using CleanArchCQRSandMediator.infra;
using CleanArchCQRSandMediator.infra.Data;
using CleanArchCQRSandMediator.infra.Persistence.InitialData;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add layer dependency    
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerGen();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.MapOpenApi();

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Initial data
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    try
    {
        if (serviceProvider == null) throw new ArgumentNullException();

        var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
        if (context == null) throw new InvalidOperationException();

        SeedData.Initialize(context);
    }
    catch (Exception ex)
    {
        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}

app.Run();
