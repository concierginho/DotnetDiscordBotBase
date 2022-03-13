using System;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DotnetDiscordBotBase.Config
{
    public class BotBaseConfig
    {
        private string _botTokenVariableName;
        private string _botPasswdVariableName;
        private string _botToken;
        private string _botPasswd;

        private IConfiguration _config;
        private IHostEnvironment _env;
        private IServiceProvider _services;
        private ILogger<BotBaseConfig> _logger;

        public BotBaseConfig(IConfiguration config,
            IHostEnvironment env,
            IServiceProvider services,
            ILogger<BotBaseConfig> logger)
        {
            if (config is null)
                throw new ArgumentNullException(nameof(config));

            if (env is null)
                throw new ArgumentNullException(nameof(env));

            if (services is null)
                throw new ArgumentNullException(nameof(services));

            _config = config;
            _env = env;
            _services = services;
            _logger = logger;

            ReadSettings();
            ReadConfig();
        }

        private void ReadSettings()
        {
            _botTokenVariableName = this.Configuration["Bot:TokenVariableName"];
            _botPasswdVariableName = this.Configuration["Bot:PasswdVariableName"];
        }

        private void ReadConfig()
        {
            if (_botTokenVariableName is null)
                throw new ArgumentNullException(nameof(_botPasswdVariableName));

            _botToken = _config[_botTokenVariableName];

            if (_botPasswdVariableName is not null)
                _botPasswd = _config[_botPasswdVariableName];
        }

        public IConfiguration Configuration { get { return _config; } }
        public IHostEnvironment Environment { get { return _env; } }
        public IServiceProvider Services { get { return _services; } }

        public string BotToken { get { return _botToken; } private set { _botToken = value; } }
        public string BotPasswd { get { return _botPasswd; } private set { _botPasswd = value; } }

        public bool DiagnosticsMode { get; set; }
        public bool AllowInnerCommands => !string.IsNullOrEmpty(_botPasswd);
    }
}
