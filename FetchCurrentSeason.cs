using Newtonsoft.Json.Linq;
using RestSharp;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace SeasonalAnime;


public class FetchCurrentSeason : IRecommendProp, IAnimeProperties, IDisposable
{
    // Singleton Pattern
    private readonly RestClient _client;
    private JObject _json_results;

    public FetchCurrentSeason(string url)
    {
        _client = new RestClient(url);
    }

    public JObject GetAllSeasons()
    {
        var request = new RestRequest("/seasons/now");
        var response = _client.Get(request);
        var data = JsonSerializer.Deserialize<JsonNode>(response.Content!)!;
	
        // JObject result = JObject.Parse(data.ToString());
        _json_results = JObject.Parse(data.ToString());

        return _json_results;

        //Console.WriteLine(_client.BuildUri(request));

        //foreach (var item in result["data"])
        //{
        //    Console.WriteLine(item["title"]);
        //}
    }

    public void Title()
    {
        foreach (var item in _json_results["data"])
        {
            
            Console.WriteLine($"Title is: {item["title"]}");
        }
    }

    public void Url()
    {
        foreach (var item in _json_results["data"])
        {
            Console.WriteLine($"URL is: {item["url"]}");
        }
    }

    public void Score()
    {
        foreach (var item in _json_results["data"])
        {
            Console.WriteLine($"Score is: {item["score"]}");
        }
    }

    // Needs to be disposed so we can dispose of the wrapped HttpClient instance.
    public void Dispose() 
    {
        _client?.Dispose();
    }
}