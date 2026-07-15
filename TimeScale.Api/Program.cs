
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System.Reflection;
using TimeScale.Api.Middlewares;
using TimeScale.Api.Validators;
using TimeScale.Application.Helpers;
using TimeScale.Application.Interfaces;
using TimeScale.Application.Services;
using TimeScale.Application.Validators;
using TimeScale.DataAccess;
using TimeScale.DataAccess.Repositories;

namespace TimeScale.Api
{
    /// <summary>
    /// Configures and starts the TimeScale web API host.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Entry point for the web application.
        /// </summary>
        /// <param name="args">Command-line arguments.</param>
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddExceptionHandler<GlobalExceptionMiddleware>();
            builder.Services.AddProblemDetails();
            builder.Services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy(), tags: ["live"])
                .AddDbContextCheck<AppDbContext>("database", tags: ["ready"]);

            builder.Services.AddValidatorsFromAssemblyContaining<UploadCsvCommandValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<ResultFilterDtoValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<UploadCsvFormValidator>();
            builder.Services.AddFluentValidationAutoValidation();

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();
            builder.Services.AddSwaggerGen(options =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(AppDbContext)));
            });

            builder.Services.AddScoped<IFileProcessingService, FileProcessingService>();
            builder.Services.AddScoped<IResultsQueryService, ResultsQueryService>();
            builder.Services.AddScoped<ICsvParser, CsvParser>();
            builder.Services.AddScoped<ICsvDomainValidator, CsvDomainValidator>();
            builder.Services.AddScoped<IResultCalculator, ResultCalculator>();
            builder.Services.AddScoped<IResultRepository, ResultRepository>();
            builder.Services.AddScoped<IUploadedFileRepository, UploadedFileRepository>();
            builder.Services.AddScoped<IValueRecordRepository, ValueRecordRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>()
                    .CreateLogger("Startup");
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                try
                {
                    await dbContext.Database.MigrateAsync();
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "Database migrations failed; attempting schema creation as fallback.");
                    await dbContext.Database.EnsureCreatedAsync();
                }
            }

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseExceptionHandler();
            app.UseAuthorization();

            app.MapControllers();
            app.MapHealthChecks("/health/live", new HealthCheckOptions
            {
                Predicate = registration => registration.Tags.Contains("live")
            });
            app.MapHealthChecks("/health/ready", new HealthCheckOptions
            {
                Predicate = registration => registration.Tags.Contains("ready")
            });

            app.Run();
        }
    }
}
