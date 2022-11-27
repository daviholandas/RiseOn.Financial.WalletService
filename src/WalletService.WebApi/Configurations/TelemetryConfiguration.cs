using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace WalletService.WebApi.Configurations;

public static class TelemetryConfiguration
{
    public static WebApplicationBuilder AddTelemetry(this WebApplicationBuilder webApplicationBuilder)
    {
        var resourceBuilder = ResourceBuilder
            .CreateDefault()
            .AddService(webApplicationBuilder.Environment.ApplicationName);

        // Logging
        webApplicationBuilder.Logging.AddOpenTelemetry(builder =>
        {
            builder.SetResourceBuilder(resourceBuilder)
                .AddConsoleExporter();
        });

        // Metrics
        webApplicationBuilder.Services.AddOpenTelemetryMetrics(metrics =>
        {
            metrics.SetResourceBuilder(resourceBuilder)
                .AddPrometheusExporter()
                .AddAspNetCoreInstrumentation()
                .AddRuntimeInstrumentation()
                .AddHttpClientInstrumentation()
                .AddEventCountersInstrumentation(config =>
                {
                    config.AddEventSources(
                        "Microsoft.AspNetCore.Hosting",
                        "System.Net.Http",
                        "System.Net.Sockets",
                        "System.Net.NameResolution",
                        "System.Net.Security");
                });
        });
        
        // Tracing
        webApplicationBuilder.Services.AddOpenTelemetryTracing(tracer =>
        {
            tracer.SetResourceBuilder(resourceBuilder)
                .AddConsoleExporter()
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation();
        });
        
        return webApplicationBuilder;
    }
}