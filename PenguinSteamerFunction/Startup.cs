using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PenguinSteamerFunction;

[assembly: FunctionsStartup(typeof(Startup))]

namespace PenguinSteamerFunction
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddOptions<UserSettings>()
                .Configure<IConfiguration>((settings, configuration) => configuration.Bind("UserSettings", settings));
        }
    }
}
