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

using CancellationTokenSource cts = new();

var feedUrl = "https://www.apkmirror.com/apk/instagram/threads-an-instagram-app/variant-%7B%22arches_slug%22%3A%5B%22arm64-v8a%22%5D%7D/feed/";

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
    item.Title = rssItem.Element("title")?.Value + " || " + rssItem.Element("pubDate")?.Value;
    item.Link = rssItem.Element("link")?.Value;
    await botActions.SendMessageAsync($"{item.Title}\n\n\n{item.Link}", cts.Token);
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