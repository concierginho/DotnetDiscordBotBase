using System;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using DotnetDiscordBotBase.Config;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DotnetDiscordBotBase.Services
{
    public class BotBaseService : BackgroundService
    {
        public BotBaseService(BotBaseConfig botBaseConfig,
            DiscordSocketClient discordClient,
            CommandService commandService,
            ILogger<BotBaseService> logger)
        {
            this.botBaseConfig = botBaseConfig;
            this.discordClient = discordClient;
            this.commandService = commandService;
            this.logger = logger;
        }

        private readonly BotBaseConfig botBaseConfig;
        private readonly DiscordSocketClient discordClient;
        private readonly ILogger<BotBaseService> logger;

        private CommandService commandService;
        public CommandService CommandService { get { return commandService; } }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            discordClient.Ready += OnReady;
            discordClient.Connected += OnConnected;
            discordClient.MessageReceived += OnMessageReceived;

            commandService.CommandExecuted += OnCommandExecuted;

            if (botBaseConfig.AllowInnerCommands)
            {
                //TODO: passwd for bot in order to allow inner commands
                // inner commands means doing changes in way bot is functioning
                // e.g. put it into diagnostics mode
                // for shards it could be putting single shard inside diagnostics mode etc.
                // this is something to think about, not necessarily a requirement
                // 1. create mechanism to distinguish normal cmd from inner cmd
                // 2. create class InnerCommands
                await commandService.AddModulesAsync(Assembly.GetEntryAssembly(), botBaseConfig.Services);
            }

            await discordClient.LoginAsync(TokenType.Bot, botBaseConfig.BotToken);
            await discordClient.StartAsync();

            await Task.Delay(-1);
        }

        private async Task OnReady()
        {
            await discordClient.SetStatusAsync(UserStatus.Online);

            logger.LogInformation("discord client is now ready to operate");
        }

        private Task OnConnected()
        {
            if (discordClient.ConnectionState.Equals(ConnectionState.Connected))
            {
                logger.LogInformation("discord client changed its state and is now: connected");
            }

            return Task.CompletedTask;
        }

        private async Task OnMessageReceived(SocketMessage msg)
        {        
            if (msg.Author.IsBot ||
                msg.Source != MessageSource.User ||
                msg is not SocketUserMessage)
            {
                return;
            }

            var msgCmdArg = ReadMessageCommandAndArgument(msg);

            if (msgCmdArg is not null)
            {
                logger.LogInformation($"correctly identified command as: {msgCmdArg.Item2} with argument {msgCmdArg.Item3}");

                await commandService.ExecuteAsync(
                    new SocketCommandContext(discordClient, msgCmdArg.Item1),
                    msgCmdArg.Item3,
                    botBaseConfig.Services
                );
            }
        }

        private Tuple<SocketUserMessage, string, string> ReadMessageCommandAndArgument(SocketMessage msg)
        {
            if (msg is not SocketUserMessage)
            {
                return null;
            }

            var message = msg as SocketUserMessage;

            var match = Regex.Match(message.Content, @"^!\w{3,15}\s{0,}", RegexOptions.IgnoreCase);

            if (match.Success)
            {
                var argument = message.Content.Substring(match.Length);

                return new Tuple<SocketUserMessage, string, string>(message, match.Value, argument);
            }

            return null;
        }

        private Task OnCommandExecuted(Optional<CommandInfo> arg1, ICommandContext arg2, IResult arg3)
        {
            throw new NotImplementedException();
        }
    }
}