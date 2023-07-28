using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram_rss_reader_bot;

var services = new ServiceCollection();
var startup = new Startup();
startup.ConfigureServices(services);
services.AddSingleton<BotActions>();
var serviceProvider = services.BuildServiceProvider();

var appsettings = serviceProvider.GetRequiredService<IConfiguration>().GetSection("TelegramBotSettings").Get<TelegramBotSettings>();

var botActions = serviceProvider.GetRequiredService<BotActions>();

using CancellationTokenSource cts = new();

await botActions.SendMessageAsync(appsettings.ChatId.ToString(), $"Hello, World!", cts.Token);

cts.Cancel();