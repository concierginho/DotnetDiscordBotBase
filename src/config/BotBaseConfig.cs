using System;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace DotnetDiscordBotBase.Config
{
    public class BotBaseConfig
    {
        public BotBaseConfig(IConfiguration configuration,
            IHostEnvironment environment, IServiceProvider services)
        {
            ReadConfig();
        }

        private void ReadConfig()
        {
            if(this.environment.IsDevelopment())
            {
                botToken = configuration[Constants.DevelopmentBotToken];
                botPasswd = configuration[Constants.DevelopmentBotPasswd];
            }
            else
            {
                botToken = configuration[Constants.ProductionBotToken];
                botPasswd = configuration[Constants.ProductionBotPasswd];
            }
        }

        private IConfiguration configuration;
        public IConfiguration Configuration { get { return configuration; } }

        private IHostEnvironment environment;
        public IHostEnvironment Environment { get { return environment; } }

        private IServiceProvider services;
        public IServiceProvider Services { get { return services; } }

        private string botToken;
        public string BotToken { get { return botToken; } private set { botToken = value; } }

        private string botPasswd;
        public string BotPasswd { get { return botPasswd; } private set { botPasswd = value; } }

        public bool AllowInnerCommands => !string.IsNullOrEmpty(botPasswd);
    }
}