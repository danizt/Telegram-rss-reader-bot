namespace Telegram_rss_reader_bot;

using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;

public class BotActions
{
  private readonly ITelegramBotClient _botClient;

  public BotActions(ITelegramBotClient botClient)
  {
    _botClient = botClient;
  }

  public async Task SendMessageAsync(string chatId, string message, CancellationToken cancellationToken)
  {
    await _botClient.SendTextMessageAsync(chatId, message, cancellationToken: cancellationToken);
  }
}