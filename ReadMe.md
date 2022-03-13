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