using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;
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

using CancellationTokenSource cts = new();

var feedUrl = configuration.GetValue<string>("TelegramBotSettings:FeedUrl");

try
{
  using var httpClient = new HttpClient();
  using var response = await httpClient.GetAsync(feedUrl, cts.Token);
  using var content = await response.Content.ReadAsStreamAsync();
  var document = XDocument.Load(content);
  var rssItems = document.Descendants("item");
  foreach (var rssItem in rssItems)
  {
    var item = new RssItem();
    item.Title = $"{rssItem.Element("title")?.Value}\n\n{rssItem.Element("pubDate")?.Value}";
    item.Link = rssItem.Element("link")?.Value;
    await botActions.SendMessageAsync($"{item.Title}\n\n\n{item.Link}", cts.Token, disablePreview: true);
  }
}
catch (Exception ex)
{
  await botActions.SendMessageAsync($"Error reading RSS feed: {ex.Message}", cts.Token);
}

cts.Cancel();


public class RssItem
{
  public string Title { get; set; }
  public string Link { get; set; }
}