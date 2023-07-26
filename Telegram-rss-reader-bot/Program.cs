using Telegram.Bot;
using Microsoft.Extensions.Configuration;
using Telegram_rss_reader_bot;

var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config/appsettings.json")
                .AddJsonFile($"config/appsettings.production.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

var appsettings = config.GetSection("TelegramBotSettings").Get<TelegramBotSettings>();

var botClient = new TelegramBotClient(appsettings.Token);

var me = await botClient.GetMeAsync();
Console.WriteLine($"Hello, World! I am user {me.Id} and my name is {me.FirstName}.");


