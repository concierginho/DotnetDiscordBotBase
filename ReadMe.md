### DotnetDiscordBotBase v1.0.1
In order to make it work properly you need to instantiate all of classes required by `BotBaseService` constructor and add `BotBaseService` as hosted service to your application.
Barrier can be shared across multiple hosted services that use `DiscordSocketClient` in order to synchronize connection, which is attempted by `BotBaseService`. After connection `DiscordSocketClient` is ready to action!

## Environment
To configure bot appropriately environment variables need to be set:
- PROD_BOT_TOKEN
- PROD_BOT_PASSWD (*optional*)
for development purposes these variables can be different to prevent breaking connection of existing production instances:
- DEV_BOT_TOKEN
- DEV_BOT_PASSWD (*optional*)

### Things to do:
- [x] make environment variable names more flexible
- [ ] configurator commands
    - [x] authentication mechanism for enabling configurator commands
    - [ ] remove module containing configurator commands after specific time
- [ ] tests
- [ ] pipeline