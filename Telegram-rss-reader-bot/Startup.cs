namespace Telegram_rss_reader_bot;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;

public class Startup
{
  public IConfiguration Configuration { get; }

  public Startup()
  {
    Configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("config/appsettings.json")
            .AddJsonFile($"config/appsettings.production.json", optional: true)
            .AddEnvironmentVariables()
            .Build();
  }

  public void ConfigureServices(IServiceCollection services)
  {
    services.AddSingleton(Configuration);

    var appSettings = Configuration.GetSection("TelegramBotSettings").Get<TelegramBotSettings>() ?? throw new InvalidOperationException("The 'TelegramBotSettings' configuration section is missing.");
    ValidateSetting(appSettings.Token, "TelegramBotSettings:Token");
    ValidateSetting(appSettings.ChatId, "TelegramBotSettings:ChatId");
    services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(appSettings.Token));
  }

  public static void ValidateSetting(string value, string settingName)
  {
    if (string.IsNullOrEmpty(value))
    {
      throw new InvalidOperationException($"The '{settingName}' configuration value is missing or empty.");
    }
  }
}
