using AirFinder.Infra.Utils.Configuration;
using AirFinder.Infra.IoC;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Rewrite;
using AirFinder.API.HealthCheck;
using AirFinder.Application.Email.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("App:Settings"));
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = "Air Finder",
            Version = "v1",
            Description = "API responsavel pelo dominio Air Finder",
            Contact = new Microsoft.OpenApi.Models.OpenApiContact
            {
                Name = "Air Finder"
            }
        });
});

builder.Services.AddControllers();

#region Local Injections
builder.Services.AddLocalServices(builder.Configuration);
builder.Services.AddLocalHttpClients(builder.Configuration);
builder.Services.AddLocalUnitOfWork(builder.Configuration);
#endregion

builder.Services.AddTransient<IMailService, MailService>();
builder.Services.AddOptions();
builder.Services.AddHealthCheckServices(builder.Configuration);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin();
        builder.AllowAnyHeader();
        builder.AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseRouting();
app.UseAuthorization();
app.UseCors("AllowAllOrigins");
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Air Finder API");
});
app.UseEndpoints(endpoints =>
{
    endpoints.MapHealthChecks("/healthcheck", new HealthCheckOptions
    {
        Predicate = _ => true,
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
    endpoints.MapHealthChecks("/health/startup");
    endpoints.MapHealthChecks("/healthz", new HealthCheckOptions
    {
        Predicate = _ => false
    });
    endpoints.MapControllers();
});
app.UseRewriter(new RewriteOptions().AddRedirect(@"^(?![\s\S])", "healthcheck"));
app.Run();
