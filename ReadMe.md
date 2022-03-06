### DotnetDiscordBotBase v1.0.1

`BotBaseService`
*requires* BotBaseConfig
*requires* DiscordSocketClient
*requires* CommandService
*requires* Barrier
*requires* ILogger<BotBaseService>

In order to make it work properly you need to instantiate all of above classes and add `BotBaseService` as hosted service to your application.
Barrier can be shared across multiple hosted services that use `DiscordSocketClient` in order to synchronize connection, which is attempted by `BotBaseService`. After connection `DiscordSocketClient` is ready to action!

### Things to do:
- [x] make environment variable names more flexible
- [ ] configurator commands
    - [x] authentication mechanism for enabling configurator commands
    - [ ] remove module containing configurator commands after specific time
- [ ] tests
- [ ] pipeline