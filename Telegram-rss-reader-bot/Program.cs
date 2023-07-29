using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using HtmlAgilityPack;
using Telegram_rss_reader_bot;

var services = new ServiceCollection();
var startup = new Startup();
startup.ConfigureServices(services);
services.AddSingleton<BotActions>();
var serviceProvider = services.BuildServiceProvider();

var botActions = serviceProvider.GetRequiredService<BotActions>();
var configuration = serviceProvider.GetRequiredService<IConfiguration>();

var feedUrl = configuration.GetValue<string>("TelegramBotSettings:FeedUrl");
var timeSpan = configuration.GetValue<int>("TelegramBotSettings:FeedCheckMinutesInterval");

using CancellationTokenSource cts = new();

var timer = new Timer(async _ =>
{
  try
  {
    using var httpClient = new HttpClient();
    using var response = await httpClient.GetAsync(feedUrl, cts.Token);
    using var content = await response.Content.ReadAsStreamAsync();
    var document = XDocument.Load(content);
    var rssItems = document.Descendants("item");

    Console.WriteLine($"[{DateTime.Now}] - Reading feed. {rssItems.Count()} items found.");
    foreach (var rssItem in rssItems)
    {
      var item = new RssItem
      {
        Title = rssItem.Element("title")?.Value,
        DateTime = DateTime.Parse(rssItem.Element("pubDate")?.Value),
        Link = rssItem.Element("link")?.Value
      };
      var properties = item.GetType().GetProperties();
      if (!properties.All(p => p.GetValue(item) == null))
      {
        await botActions.SendMessageAsync($"{item.Title}\n\n{item.DateTime}\n\n\n{item.Link}", cts.Token, disablePreview: true);
      }
    }
  }
  catch (Exception ex)
  {
    await botActions.SendMessageAsync($"Error reading RSS feed: {ex.Message}", cts.Token);
  }
}, null, TimeSpan.Zero, TimeSpan.FromMinutes(timeSpan));

Console.ReadLine();
cts.Cancel();
timer.Dispose();