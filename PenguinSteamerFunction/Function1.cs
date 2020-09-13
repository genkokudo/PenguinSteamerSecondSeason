using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Functions.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace PenguinSteamerFunction
{
    public class Function1
    {
        private readonly IOptions<UserSettings> _settings;

        public Function1(IOptions<UserSettings> settings)
        {
            _settings = settings;
        }

        [FunctionName("Function1")]
        public void Run([TimerTrigger("0 */5 * * * *", RunOnStartup = true)]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"‚ ‚È‚½‚Ì–¼‘O‚Í{_settings.Value.Name}");
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}
