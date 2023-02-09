using OpenTelemetry;
using OpenTelemetry.Instrumentation.AspNetCore;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;


namespace DigitalBanking.ServiceExtensions
{
    public static partial class Observability
    {
        public static WebApplicationBuilder UseObservibility(this WebApplicationBuilder builder)
        {
            //var redactionProcessor = new RedactionProcessor(builder);

            builder.Services.AddOpenTelemetryTracing((telemetryBuilder) =>
            {
            telemetryBuilder.AddAspNetCoreInstrumentation((options) => options.Enrich
                = (activity, eventName, rawObject) =>
                {
                    if (eventName.Equals("OnStartActivity"))
                    {
                        if (rawObject is HttpRequest httpRequest)
                        {
                            activity.SetTag("requestProtocol", httpRequest.Protocol);
                        }
                    }
                    else if (eventName.Equals("OnStopActivity"))
                    {
                        if (rawObject is HttpResponse httpResponse)
                        {
                            activity.SetTag("responseLength", httpResponse.ContentLength);
                        }
                    }
                }).AddJaegerExporter();

                telemetryBuilder.AddEntityFrameworkCoreInstrumentation((options) =>
                {
                    options.SetDbStatementForText = true;
                    options.SetDbStatementForStoredProcedure = true;
                });
                telemetryBuilder.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("Digital Banking APIs"));


               // telemetryBuilder.AddProcessor();
                telemetryBuilder.AddHttpClientInstrumentation();
            });
          
            return builder;
        }
    }
}
