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
    public int Popularity { get; set; }
    // Genre is not a single value. Contains multiple json value.
    public string Genres { get; set; }
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

    private List<string> anime_genre = new List<string>();  // Store this onto dictionary. 
	// Hashmap to store title - popularity. Popularity as key, title as value.
    private Dictionary<int, string> _anime_dict = new Dictionary<int, string>();
    
   
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

    public void FilterAnimes()
    {
		foreach (var item in _json_results["data"])
		{
			_anime_prop.Title = item["title"].ToString();
			//_anime_prop.Synopsis = item["synopsis"];
			//_anime_prop.Img = item["images"]["jpg"]["image_url"];
			//_anime_prop.Url = item["url"];
			_anime_prop.Popularity = (int)item["popularity"];
            _anime_prop.Genres = item["genres"].ToString();

            //DisplayJsonValue(_anime_prop.Genres);

            // Add anime properties into a list to save the anime entry.
            // Take title and popularity values in the list. Sort by popularity.
            // Then search using api for the name of the anime for the rest of the information.
            if (!_anime_dict.ContainsKey(_anime_prop.Popularity))
			{
				_anime_dict.Add(_anime_prop.Popularity, _anime_prop.Title);
			}
			
		}

        // DisplayAnimeDictionary();
        SortPopularityAnime();
	}

    // Sorts anime list from most popular to least (lowest number = most popular).
    public void SortPopularityAnime()
    {
        var sorted_dict = from keys in _anime_dict orderby keys.Key ascending select keys;

        // Print sorted dictionary list.
        //foreach (KeyValuePair<int, string> kvp in sorted_dict)
        //{
        //    Console.WriteLine(string.Format("Key = {0}, Value = {1}", kvp.Key, kvp.Value));
        //}
        //      Console.WriteLine("--------");
        //      Console.WriteLine("Number of Items in Dictionary: ", sorted_dict.Count());
        //Console.WriteLine("--------");

        FilterByUserGenre();
    }

    // Filter the sorted dictionary based on the user's genre.
    public void FilterByUserGenre()
    {
        // var user_genre_list = _baseline_data.GetUserGenre();

        foreach (KeyValuePair<int, string> kvp in _anime_dict)
        {
            var anime_name = _anime_dict[kvp.Key];
            //SearchByName(anime_name);
            Console.WriteLine(anime_name);
        }
    }

    public void SearchByName(string name)
    {
        // search api using the name.
	}

	// Testing purposes. Print things from json value filtering
	public void DisplayJsonValue<Thing>(Thing json_value)
    {
        Console.WriteLine(json_value);
        Console.WriteLine("----------------");
    }

    // Testing purposes. Print items in anime list.
    public void DisplayAnimeDictionary()
    {
        foreach (KeyValuePair<int, string> kvp in _anime_dict)
        {
            Console.WriteLine(string.Format("Key = {0}, Value = {1}", kvp.Key, kvp.Value));
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