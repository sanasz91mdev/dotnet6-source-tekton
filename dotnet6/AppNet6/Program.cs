using DigitalBanking.ServiceExtensions;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Net.Http.Headers;
using Prometheus;
using Serilog;

namespace DigitalBanking.Global
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Wire up Collection of services that application needs
            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration.AddJsonFile(Path.Combine(AppContext.BaseDirectory, "appsettings.json"), false, true);
            builder.Services.AddMemoryCache();
            builder.Services.AddCarter();            
            builder.AddSwagger();
            builder.AddSerilog();
            builder.Services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost;
            });
            builder.AddDbContexts();
            builder.UseResourceServices();
            builder.Services.AddHostedService<StartupBackgroundService>();
            builder.Services.AddSingleton<StartupHealthCheck>();
            builder.Services.AddHealthChecks()
                .AddCheck<StartupHealthCheck>(
                    "Startup",
                    tags: new[] { "ready" })
                //.AddOracle(
                //    builder.Configuration.GetConnectionString("DatabaseConnection"))
                .AddCheck<SampleHealthCheck>("Sample", tags: new[] { "live" })
                .ForwardToPrometheus();
            
            builder.UseObservibility();

            builder.Services.AddHttpClient("GitHub", httpClient =>
            {
                httpClient.BaseAddress = new Uri("https://api.github.com/");

                // using Microsoft.Net.Http.Headers;
                // The GitHub API requires two headers.
                httpClient.DefaultRequestHeaders.Add(
                    HeaderNames.Accept, "application/vnd.github.v3+json");
                httpClient.DefaultRequestHeaders.Add(
                    HeaderNames.UserAgent, "HttpRequestsSample");
            }).UseHttpClientMetrics();


            //Wire up middleware for request processing .. Order is very important
            var app = builder.Build();
            app.UseForwardedHeaders();
            app.UseMetricServer();
            app.UseMiddleware<RequestLogContextMiddleware>();
            app.UseMiddleware<RequestResponseLoggingMiddleware>();

            app
        .UseExceptionHandling(app.Environment)
        .UseSwaggerEndpoints(routePrefix: string.Empty);

            //Map all endpoints that this API Server will use

            app.MapGet("/", () => "Hello World!");
            app.MapCarter();

            //register all services then do a health check

            app.MapHealthChecks("/healthz/ready", new HealthCheckOptions
            {
                Predicate = healthCheck => healthCheck.Tags.Contains("live")
            });

            app.MapHealthChecks("/healthz/live", new HealthCheckOptions
            {
                Predicate = healthCheck => healthCheck.Tags.Contains("live")
            });


            app.Run();




        }
    }
}