using FluentValidation;
using GeniusExchangeConverter.Api.Middlewares;
using GeniusExchangeConverter.Application.Configuration;
using GeniusExchangeConverter.Infrastructure.Configuration;
using Microsoft.OpenApi.Models;

namespace GeniusExchangeConverter.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options => options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "Currency Conversion API",
            Description = "API for converting currency amounts"
        }));

        InfrastructureConfiguration.ConfigureInfrastructure(builder.Services, builder.Configuration);
        ApplicationConfiguration.ConfigureApplication(builder.Services);

        builder.Services.AddAutoMapper(typeof(Program));
        builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

        var app = builder.Build();

        InfrastructureConfiguration.ApplyMigrations(app.Services);

        app.UseMiddleware<CustomExceptionMiddleware>();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}