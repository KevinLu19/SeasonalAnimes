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

* Task:
 - Ability to write both english and japanese names on the search.
 */
public class FetchCurrentSeason :  IDisposable
{
    // Singleton Pattern
    private readonly RestClient _client;

    private JObject _json_results;
    private JObject _anime_name_result;
    private Anime _anime_prop;

    
	// Hashmap to store title - popularity. Popularity as key, title as value.
    // List of string will be used to store the json meta data from the api.
    private Dictionary<int, string> _anime_dict = new Dictionary<int, string>();
   
	public FetchCurrentSeason(string url)
    {
        _client = new RestClient(url);
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
            //_anime_prop.Genres = item["genres"].ToString();

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
		//FilterByUserGenre();
		SortPopularityAnime();
	}

    // Sorts anime list from most popular to least (lowest number = most popular).
    /*
        Popularity = Amount of people that have it on their list (completed, watching, on hold, etc). 
     */
    public void SortPopularityAnime()
    {
        var sorted_dict = from keys in _anime_dict orderby keys.Key ascending select keys;
        
        // Used as an anime selector for the menu.
        int anime_count = 1;
        Dictionary<int, string> dict_saved_option = new Dictionary<int, string>();

		Console.WriteLine("+++++++++");
		Console.WriteLine("These are listed from most popular to least popular");
		Console.WriteLine("+++++++++");

		// Print sorted dictionary list.
		foreach (KeyValuePair<int, string> kvp in sorted_dict)
        {
            //Console.WriteLine(string.Format("Key = {0}, Value = {1}", kvp.Key, kvp.Value));
            Console.WriteLine($"{anime_count} : Popular # {kvp.Key} : {kvp.Value}");
            dict_saved_option.Add(anime_count, kvp.Value);          // Dictionary stores anime count to anime title. This will be used to help select anime number on the menu.
            anime_count++;
        }
        Console.WriteLine("+++++++++");
        Console.WriteLine($"Number of Animes Present In This Season: {_anime_dict.Count}");
		Console.WriteLine("+++++++++");
		
        PrintOptions(dict_saved_option);
	}

    // Filter the sorted dictionary based on the user's genre.
    // Unfinished. Need to figure out how to get genre and store it somewhere without the use of databases.
    public void FilterByUserGenre()
    {
        // var user_genre_list = _baseline_data.GetUserGenre();

        foreach (KeyValuePair<int, string> kvp in _anime_dict)
        {
            var anime_name = _anime_dict[kvp.Key];
            //SearchByName(anime_name);
            Console.WriteLine(anime_name);
            Console.WriteLine(_anime_prop.Genres);
        }
    }

	// Testing purposes. Print things from json value filtering
	public void DisplayJsonValue<Thing>(Thing json_value)
    {
        Console.WriteLine(json_value);
        Console.WriteLine("----------------");
    }

    // Option to have more indepth detail of an interested anime on the given sorted popularity list.
    public void PrintOptions(Dictionary<int, string> dict_saved_selector)
    {
		Console.WriteLine("=========================================");
        Console.WriteLine("Here are your options for more details");
        Console.WriteLine("1 - Indepth information for a given listed anime.");
        Console.WriteLine("2 - Manually input which genre I want.");
        Console.WriteLine("q - Exit Menu");
        Console.WriteLine("=========================================");
        Console.Write("Please enter in a number or q to quit: ");

		string user_input = Console.ReadLine()!;

		if (user_input == "q" || user_input == "Q")
        {
            Console.WriteLine("Closing project.");
            Environment.Exit(0);
        }
        else if (user_input == "1")
        {
            Console.Write("Enter the number associated next to the listed anime to get more info: ");
			int input = Convert.ToInt32(Console.ReadLine())!;
            InDepthAnimeDescription(input, dict_saved_selector);
        }
    }
    // Expand on the number 1 menu option.
    public void InDepthAnimeDescription(int user_option, Dictionary<int, string> dict_saved_selector)
    {
        // Turn numerical key back into anime title from dictionary.
        var anime_name = dict_saved_selector[user_option];

        // Limit = 1 -> only show one result instead of 15+.
		var request = new RestRequest($"/anime?q={anime_name}&limit=1");
		var response = _client.Get(request);
        var data = JsonSerializer.Deserialize<JsonNode>(response.Content!)!;

        //Console.WriteLine(_client.BuildUri(request));

        _anime_name_result = JObject.Parse(data.ToString());

        Console.WriteLine(_anime_name_result["data"]);
    }

    //Testing purposes.Print items in anime list.
    public void DisplayAnimeDictionary()
    {
        foreach (KeyValuePair<int, string> kvp in _anime_dict)
        {
            Console.WriteLine(string.Format("Key = {0}, Value = {1}", kvp.Key, kvp.Value));
        }
    }

    // Needs to be disposed so we can dispose of the wrapped HttpClient instance.
    public void Dispose() 
    {
        _client?.Dispose();
    }
}