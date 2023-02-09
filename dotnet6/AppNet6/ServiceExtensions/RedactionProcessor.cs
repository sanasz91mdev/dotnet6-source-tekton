using OpenTelemetry;
using Services;
using System.Diagnostics;

namespace DigitalBanking.ServiceExtensions
{
    public class RedactionProcessor:BaseProcessor<Activity>
    {
        Mask maskService;
        public RedactionProcessor(WebApplicationBuilder builder)
        {
            var  serviceProvider = builder.Services.BuildServiceProvider(validateScopes: false);
            var mask = serviceProvider.GetService<Mask>();
            maskService = mask;

        }
        IConfiguration config = new ConfigurationBuilder()
                    .AddJsonFile("masking.json", optional: false, reloadOnChange: true).Build();

        public override void OnStart(Activity activity)
        {
        }

        public  override void OnEnd(Activity activity)
        {
            if (activity.Parent?.DisplayName != "/index.html") {
                var blackList = maskService.getMaskedList();
                var tags = activity.Tags;
                var toRemove = tags.ToList().Where(x => blackList.Contains(x.Key, StringComparer.OrdinalIgnoreCase));

                foreach (var item in toRemove)
                {
                    activity.SetTag(item.Key, null);
                }
                Console.WriteLine($"OnEnd: {activity.DisplayName}");
            }
        }
    }
}
