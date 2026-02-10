
using Microsoft.EntityFrameworkCore;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System.Reflection;
using TimeScale.Api.Middlewares;
using TimeScale.Application.Helpers;
using TimeScale.Application.Interfaces;
using TimeScale.Application.Services;
using TimeScale.DataAccess;
using TimeScale.DataAccess.Repositories;

namespace TimeScale.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddExceptionHandler<GlobalExceptionMiddleware>();
            builder.Services.AddProblemDetails();

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
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
            builder.Services.AddScoped<IFileProcessingService, FileProcessingService>();
            builder.Services.AddScoped<ICsvParser, CsvParser>();
            builder.Services.AddScoped<ICsvDomainValidator, CsvDomainValidator>();
            builder.Services.AddScoped<IResultCalculator, ResultCalculator>();
            builder.Services.AddScoped<IResultRepository, ResultRepository>();
            builder.Services.AddScoped<IUploadedFileRepository, UploadedFileRepository>();
            builder.Services.AddScoped<IValueRecordRepository, ValueRecordRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseExceptionHandler();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
