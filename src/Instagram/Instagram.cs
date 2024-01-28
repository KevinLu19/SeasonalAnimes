using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AngleSharp;
using AngleSharp.Html.Parser;

namespace SeasonalAnime.src.Instagram;
internal class Insta
{
    private readonly string _url = "";

    public Insta(string username)
    {
        _url = $"https://www.instagram.com/{username}";

    }

    public async Task ScrapeInsta()
    {
		using (var httpClient = new HttpClient())
		{
			// Send a GET request to the Instagram profile page
			var response = await httpClient.GetStringAsync(_url);

			// Parse HTML content using AngleSharp
			var context = BrowsingContext.New(Configuration.Default);
			var document = await context.OpenAsync(req => req.Content(response));

			// Extract the number of followers (example)
			var followersNode = document.QuerySelector("meta[property='og:description']");
			if (followersNode != null)
			{
				string content = followersNode.GetAttribute("content");
				string followersCount = content.Split(' ')[0];
				Console.WriteLine($"Followers: {followersCount}");
			}
			else
			{
				Console.WriteLine("Followers count not found.");
			}
		}
	}
}
