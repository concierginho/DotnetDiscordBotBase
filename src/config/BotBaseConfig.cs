using System;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace DotnetDiscordBotBase.Config
{
    public class BotBaseConfig
    {
        public BotBaseConfig(IConfiguration configuration,
            IHostEnvironment environment,
            IServiceProvider services)
        {
            this.configuration = configuration;
            this.environment = environment;
            this.services = services;

            ReadConfig();
        }

        private void ReadConfig()
        {
            if (this.environment.IsDevelopment())
            {
                botToken = configuration[DEV_BOT_TOKEN];
                botPasswd = configuration[DEV_BOT_PASSWD];
            }
            else
            {
                botToken = configuration[PROD_BOT_TOKEN];
                botPasswd = configuration[PROD_BOT_PASSWD];
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

        private bool diagnosticsMode;
        public bool DiagnosticsMode
        {
            get { return diagnosticsMode; }
            set { diagnosticsMode = value; }
        }

        public bool AllowInnerCommands => !string.IsNullOrEmpty(botPasswd);

        public const string DEV_BOT_TOKEN = "DEV_BOT_TOKEN";
        public const string PROD_BOT_TOKEN = "PROD_BOT_TOKEN";
        public const string DEV_BOT_PASSWD = "DEV_BOT_PASSWD";
        public const string PROD_BOT_PASSWD = "PROD_BOT_PASSWD";
    }
}
