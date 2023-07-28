namespace Telegram_rss_reader_bot;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddSingleton(Configuration);

      var appSettings = Configuration.GetSection("TelegramBotSettings").Get<TelegramBotSettings>();
      services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(appSettings.Token));
    }
}
