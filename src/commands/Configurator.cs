using System.Threading.Tasks;

using Discord.Commands;

using DotnetDiscordBotBase.Config;

using Microsoft.Extensions.Logging;

namespace DotnetDiscordBotBase.Commands
{
    public class Configurator : ModuleBase
    {
        public Configurator(BotBaseConfig configuration,
            ILogger<Configurator> logger)
        {
            this.configuration = configuration;
        }

        private readonly BotBaseConfig configuration;
        private readonly ILogger<Configurator> logger;

        public async Task OnLogin([Remainder] string passwd)
        {
            if (passwd.Equals(configuration.BotPasswd))
            {
                logger.LogInformation("configurator has been successfully authenticated");
            }
        }
    }
}