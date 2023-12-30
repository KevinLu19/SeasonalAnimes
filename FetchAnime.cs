using Newtonsoft.Json.Linq;
using RestSharp;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace SeasonalAnime;


// Create a model for Anime generic type.
public record Anime(string title, string url, string image);

public class FetchAnime : IDisposable
{
    // Singleton Pattern
    private readonly RestClient _client;

    public FetchAnime(string url)
    {
        _client = new RestClient(url);
    }

    public void GetAllSeasons()
    {
        var request = new RestRequest("/seasons/now");
        var response = _client.Get(request);
        var data = JsonSerializer.Deserialize<JsonNode>(response.Content!)!;
	
        JObject result = JObject.Parse(data.ToString());

        //Console.WriteLine(_client.BuildUri(request));

        foreach (var item in result["data"])
        {
            Console.WriteLine(item["title"]);
        }

    }

    // Needs to be disposed so we can dispose of the wrapped HttpClient instance.
    public void Dispose() 
    {
        _client?.Dispose();
    }
}