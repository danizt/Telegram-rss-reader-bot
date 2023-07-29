# Telegram RSS Reader Bot

This is a simple Telegram bot that reads an RSS feed and sends new items to a Telegram chat.

## Getting Started

To use this bot, you will need to create a Telegram bot and obtain an API token. You will also need to provide the URL of the RSS feed you want to read.

### Prerequisites

- .NET 5.0 or later
- A Telegram bot API token
- The URL of an RSS feed

### Installing

1. Clone this repository to your local machine.
2. Open the `appsettings.json` file and replace the `TelegramBotSettings:ApiToken` value with your Telegram bot API token.
3. Open the `appsettings.json` file and replace the `TelegramBotSettings:ChatId` value with the ID of the Telegram chat or channel you want to send messages to.
4. Replace the `TelegramBotSettings:FeedUrl` value with the URL of the RSS feed you want to read.
5. Open a terminal or command prompt and navigate to the project directory.
6. Run the following command to start the bot:

```bash
dotnet run
```

## Usage

Once the bot is running, you can send a message to the bot's chat to start receiving new items from the RSS feed.

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details.
