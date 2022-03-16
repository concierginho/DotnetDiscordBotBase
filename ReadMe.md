## DotnetDiscordBotBase
Library created in order to save time writing bot with usage of *.NET*.
## How to use it?
You only need to instantiate `BotBaseService` and start it.
## What does it do?
The library loads default configuration e.g. bot token and creates default mechanism for connection and reading commands from channels.
It is capable of understanding commands, reading its names and passed arguments.
This allow us to create new command classed with ease.

---
## Usage details
The library uses environment variables to inject sensitive data. It does this because each bot needs a unique token that should not be shared. It also means that many instances can be created on single machine because each one of them can use different environment variable names.
To rename an environment variable, simply change the values in the following files:

**1. Production Environment**

    appsettings.Production.json
**2. Development Environment**

    appsettings.Development.json
**Path to Bot Token property:**

    "Bot:TokenVariableName"
**Path to Bot Passwd property:**

    "Bot:PasswdVariableName"
If you do not have such files, you will need to create them.
Example ***appsettings.Development.json*** file:

    {
        "Logging": {
            "LogLevel": {
                "Default": "Information"
         }
        },
        "Bot": {
            "TokenVariableName": "DEV_BOT_TOKEN",
            "PasswdVariableName": "DEV_BOT_PASSWD"
    }

---

## Adding commands
To add your custom class with commands you need to create class with chosen name and inherit from `ModuleBase`.

e.g. you want to have command `!MyBot help`. Your class will look this:

```
public class MyBot : ModuleBase
{
    [Command("help")]
    public Task OnHelp()
    {
        await this.Context.Channel.SendMessageAsync("help...");
    }
}
```