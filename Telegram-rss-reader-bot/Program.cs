using Telegram.Bot;
using Microsoft.Extensions.Configuration;
using Telegram_rss_reader_bot;
using Telegram.Bot.Types;
using Microsoft.Extensions.DependencyInjection;


var services = new ServiceCollection();
var startup = new Startup(new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config/appsettings.json")
                .AddJsonFile($"config/appsettings.production.json", optional: true)
                .AddEnvironmentVariables()
                .Build());
startup.ConfigureServices(services);
var serviceProvider = services.BuildServiceProvider();

var appsettings = serviceProvider.GetRequiredService<IConfiguration>().GetSection("TelegramBotSettings").Get<TelegramBotSettings>();


var botClient = new TelegramBotClient(appsettings.Token);
using CancellationTokenSource cts = new();
var me = await botClient.GetMeAsync();

await SendMessageAsync(appsettings.ChatId.ToString(), botClient, cts.Token, $"Hello, World! I am user {me.Id} and my name is {me.FirstName}.");

cts.Cancel();


async Task SendMessageAsync(string chatId, ITelegramBotClient botClient, CancellationToken cancellationToken, string messageText)
{
  Message message = await botClient.SendTextMessageAsync(
    chatId: chatId,
    text: messageText,
    cancellationToken: cancellationToken);
}



