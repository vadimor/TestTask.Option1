using Microsoft.EntityFrameworkCore;
using TestTask.Option1.Data;
using TestTask.Option1.Data.Interfaces;
using TestTask.Option1.Filters;
using TestTask.Option1.Helper;
using TestTask.Option1.Helper.Interfaces;
using TestTask.Option1.Repositories;
using TestTask.Option1.Repositories.Interfaces;
using TestTask.Option1.Services;
using TestTask.Option1.Services.Interfaces;

var config = GetConfiguration();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(
        opt => opt.Filters.Add<HttpGlobalExceptionFilter>()
    ).AddJsonOptions(
        opt => opt.JsonSerializerOptions.WriteIndented = true
    );


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddDbContextFactory<ApplicationDbContext>(opts => opts.UseSqlServer(config["ConnectionString"]));
builder.Services.AddScoped<IDbContextWrapper<ApplicationDbContext>, DbContextWrapper<ApplicationDbContext>>();

builder.Services.AddTransient<IDeviceRepository, ApplicationRepository>();
builder.Services.AddTransient<IExperimentRepository, ApplicationRepository>();
builder.Services.AddTransient<IExperimentValueRepository, ApplicationRepository>();
builder.Services.AddTransient<ISelectionRepository, ApplicationRepository>();
builder.Services.AddTransient<IExperimentManageService, ExperimentManageService>();
builder.Services.AddTransient<IRandomWrapper, RandomWrapper>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

CreateDbIfNotExist(app);

app.Run();

IConfiguration GetConfiguration()
{
    var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables();

    return builder.Build();
}

void CreateDbIfNotExist(IHost host)
{
    using (var scope = host.Services.CreateScope())
    {
        var services = scope.ServiceProvider;

        try
        {
            var context = services.GetRequiredService<ApplicationDbContext>();

            DbInitializer.InitializeAsync(context).Wait();
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred creating the DB.");
        }
    }
}
