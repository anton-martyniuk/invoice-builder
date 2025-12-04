using InvoiceBuilder.Host.Seeding;
using Modules.Common.API.Extensions;
using Modules.Common.Infrastructure.Database;

var builder = WebApplication.CreateBuilder(args);

IronPdf.License.LicenseKey = builder.Configuration["IronPdfLicenseKey"];

builder.Services.AddWebHostDependencies();

builder.AddCoreHostLogging();

builder.Services.AddCoreWebApiInfrastructure();

builder.Services.AddCoreInfrastructure(builder.Configuration,
[
    InvoicesModuleRegistration.ActivityModuleName
]);

builder.Services
	.AddUsersModule(builder.Configuration)
	.AddInvoicesModule(builder.Configuration);

// CORS: allow any origin in Development to enable local frontend dev
const string devCorsPolicy = "DevCorsPolicy";

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(devCorsPolicy, policy =>
            policy
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
                // Expose headers so browser JS can read them (e.g., Content-Disposition for file downloads)
                .WithExposedHeaders("Content-Disposition", "content-disposition"));
    });
}

var app = builder.Build();

// Run migrations in DEVELOPMENT mode
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    await scope.MigrateModuleDatabasesAsync();

    var userSeedService = scope.ServiceProvider.GetRequiredService<UserSeedService>();
    await userSeedService.SeedUsersAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(devCorsPolicy);
}

app.UseAuthentication();
app.UseAuthorization();

app.UseModuleMiddlewares();

app.MapApiEndpoints();

await app.RunAsync();
