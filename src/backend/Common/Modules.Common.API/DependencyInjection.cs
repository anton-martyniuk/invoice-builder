using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.OpenApi;
using Modules.Common.API.ErrorHandling;
using Serilog;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

[SuppressMessage("Design", "MA0051:Method is too long")]
public static class DependencyInjection
{
    public static IServiceCollection AddCoreWebApiInfrastructure(this IServiceCollection services)
    {
        services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen(options =>
            {
	            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
	            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	            {
		            In = ParameterLocation.Header,
		            Description = "JWT Authorization header. Enter: {token} (without Bearer)",
		            Name = "Authorization",
		            Type = SecuritySchemeType.Http,
		            BearerFormat = "JWT",
		            Scheme = "Bearer"
	            });

	            options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
	            {
		            [new OpenApiSecuritySchemeReference("bearer", document)] = []
	            });
            });

        services
            .AddExceptionHandler<GlobalExceptionHandler>()
            .AddProblemDetails();

        services.Configure<JsonOptions>(opt =>
        {
            opt.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        return services;
    }

    public static void AddCoreHostLogging(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, loggerConfig) =>
            loggerConfig.ReadFrom.Configuration(context.Configuration));
    }
}
