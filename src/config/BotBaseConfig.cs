using System;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace DotnetDiscordBotBase.Config
{
    public class BotBaseConfig
    {
        private string botTokenVariableName;
        private string botPasswdVariableName;

        public BotBaseConfig(IConfiguration configuration,
            IHostEnvironment environment,
            IServiceProvider services)
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
    }
}
