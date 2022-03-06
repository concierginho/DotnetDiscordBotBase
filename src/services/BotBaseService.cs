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
    public class BotBaseService : IHostedService
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
                logger.LogInformation("synchronizing with other services...");
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

        private async Task OnCommandExecuted(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            if (!command.IsSpecified)
            {
                return;
            }

            if (botBaseConfig.DiagnosticsMode)
            {
                if (result.IsSuccess)
                {
                    await context.Message.AddReactionAsync(new Emoji("🔆"));
                    logger.LogInformation($"command has been executed successfully: {command.Value.Name}");
                }
                else
                {
                    await context.Message.AddReactionAsync(new Emoji("🌧️"));
                    logger.LogError($"unable to execute command: {command.Value.Name} due to: {result.ErrorReason}");
                }
            }
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            discordClient.Ready += OnReady;
            discordClient.Connected += OnConnected;
            discordClient.MessageReceived += OnMessageReceived;

            commandService.CommandExecuted += OnCommandExecuted;

            if (botBaseConfig.AllowInnerCommands)
            {
                await commandService.AddModulesAsync(Assembly.GetEntryAssembly(), botBaseConfig.Services);
            }

            await discordClient.LoginAsync(TokenType.Bot, botBaseConfig.BotToken);
            await discordClient.StartAsync();

            await Task.Delay(-1);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await discordClient.StopAsync();
            await discordClient.DisposeAsync();
        }
    }
}
