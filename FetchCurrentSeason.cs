using Newtonsoft.Json.Linq;
using RestSharp;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace SeasonalAnime;

public class Anime
{
    public string Title { get; set; }
    public string Synopsis { get; set; }
    public string Img { get; set; }
    public string Url { get; set; }

    // With popularity, the smaller the number, the more popular it is
    // According to myanimelist.
    public string Popularity { get; set; }
}

/*
 Grabs list of anime each current season using api. 
Within those results, return back all the animes that would deem suitable 
from the baseline information. 

Easiest solution: - check score of given anime. If it is above or equal to baseline
keep in a list. Next would be to filter from genre.
 */
public class FetchCurrentSeason : IRecommendProp, IAnimeProperties, IDisposable
{
    // Singleton Pattern
    private readonly RestClient _client;
    private JObject _json_results;

    private UserProfile _baseline_data;
    private Anime _anime_prop;

    public FetchCurrentSeason(string url)
    {
        _client = new RestClient(url);
        _baseline_data = new UserProfile();
        _anime_prop = new Anime();
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

    // Filter out all the anime given based on the baseline input.
    public void FilterAnimes()
    {
        var user_genre_list = _baseline_data.GetUserGenre();
        List<String> anime_list = new List<String>(); // List of Anime class objects.

        foreach (var user_genre in user_genre_list)
        {
            foreach (var item in _json_results["data"])
            {
                _anime_prop.Title = item["title"].ToString();
                _anime_prop.Synopsis = item["synopsis"].ToString();
                _anime_prop.Img = item["images"]["jpg"]["image_url"].ToString();
                _anime_prop.Url = item["url"].ToString();
                _anime_prop.Popularity = item["popularity"].ToString();

                // DisplayJsonValue(_anime_prop.Img);

                // Add anime properties into a list to save the anime entry.
                anime_list.Add(_anime_prop.Title);
                anime_list.Add(_anime_prop.Synopsis);
                anime_list.Add(_anime_prop.Url);
                anime_list.Add(_anime_prop.Popularity);
                anime_list.Add("-----------");
            }

        }

        // DisplayAnimeList(anime_list);
    }

    // Sorts anime list from most popular to least (lowest number = most popular).
    public void SortPopularityAnime(String popularity)
    {

    }

    // Testing purposes. Print things from json value filtering
    public void DisplayJsonValue<Thing>(Thing json_value)
    {
        Console.WriteLine(json_value);
        Console.WriteLine("----------------");
    }

    // Testing purposes. Print items in anime list.
    public void DisplayAnimeList(List<String> list)
    {
        foreach (var item in list)
        {
            Console.WriteLine(item);
        }
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