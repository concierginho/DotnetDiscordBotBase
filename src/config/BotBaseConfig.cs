using System;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DotnetDiscordBotBase.Config
{
    public class BotBaseConfig
    {
        private string botTokenVariableName;
        private string botPasswdVariableName;
        private string botToken;
        private string botPasswd;
        private readonly IConfiguration configuration;
        private readonly IHostEnvironment environment;
        private readonly IServiceProvider services;


        public BotBaseConfig(IConfiguration configuration,
            IHostEnvironment environment,
            IServiceProvider services,
            ILogger<BotBaseConfig> logger)
        {
            this.configuration = configuration;
            this.environment = environment;
            this.services = services;

            ReadSettings();
            ReadConfig();
        }

        private void ReadSettings()
        {
            this.botTokenVariableName = this.Configuration["Bot:TokenVariableName"];
            this.botPasswdVariableName = this.Configuration["Bot:PasswdVariableName"];
        }

        private void ReadConfig()
        {
            botToken = configuration[this.botTokenVariableName];
            botPasswd = configuration[this.botPasswdVariableName];
        }

        public IConfiguration Configuration { get { return configuration; } }
        public IHostEnvironment Environment { get { return environment; } }
        public IServiceProvider Services { get { return services; } }

        public string BotToken { get { return botToken; } private set { botToken = value; } }
        public string BotPasswd { get { return botPasswd; } private set { botPasswd = value; } }

        public bool DiagnosticsMode { get; set; }
        public bool AllowInnerCommands => !string.IsNullOrEmpty(botPasswd);
    }
}
