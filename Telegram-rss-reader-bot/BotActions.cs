using Microsoft.Extensions.Configuration;
using Telegram.Bot;

public class BotActions
{
    private readonly ITelegramBotClient _botClient;
    private readonly IConfiguration _configuration;

    public BotActions(ITelegramBotClient botClient, IConfiguration configuration)
    {
        _botClient = botClient;
        _configuration = configuration;
    }

    public async Task SendMessageAsync(string message, CancellationToken cancellationToken)
    {
        var chatId = _configuration.GetValue<long>("TelegramBotSettings:ChatId");
        await _botClient.SendTextMessageAsync(chatId.ToString(), message, cancellationToken: cancellationToken);
    }
}