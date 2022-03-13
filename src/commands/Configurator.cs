using System.Threading.Tasks;

using Discord.Commands;

using DotnetDiscordBotBase.Config;

using Microsoft.Extensions.Logging;

namespace DotnetDiscordBotBase.Commands
{
    public class Configurator : ModuleBase
    {
        private readonly ILogger<Configurator> logger;
        private readonly BotBaseConfig configuration;

        public Configurator(BotBaseConfig configuration,
            ILogger<Configurator> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
        }

        public Task OnLogin([Remainder] string passwd)
        {
            if (passwd.Equals(configuration.BotPasswd))
            {
                logger.LogInformation("configurator has been successfully authenticated");
            }
            return Task.CompletedTask;
        }
    }
}