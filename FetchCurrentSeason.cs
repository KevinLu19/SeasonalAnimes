using Newtonsoft.Json.Linq;
using RestSharp;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace SeasonalAnime;

public class Anime
{
    public string Title { get; set; }
    public string Synopsis { get; set; }
    public string Img { get; set; }
    public string Url { get; set; }

    public string Score { get; set; }
    public string Score_By { get; set; }
    public string Members { get; set; }
    public string Rank { get; set; }
    public string Season { get; set; }
    public string Studio { get; set; }
    public string Themes { get; set; }
    public string Demographic { get; set; }
    public string Source { get; set; }
    public string Type { get; set; }

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
    private Dictionary<string, string> _genre_dict = new Dictionary<string, string>();      // Key = anime title, value = genre.
   
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
            _anime_prop.Genres = item["genres"].ToString();

            //DisplayJsonValue(_anime_prop.Genres);

            // Add anime properties into a list to save the anime entry.
            // Take title and popularity values in the list. Sort by popularity.
            // Then search using api for the name of the anime for the rest of the information.
            if (!_anime_dict.ContainsKey(_anime_prop.Popularity))
			{
				_anime_dict.Add(_anime_prop.Popularity, _anime_prop.Title);
                _genre_dict.Add(_anime_prop.Title, _anime_prop.Genres);				
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

        PrintOptions(dict_saved_option, anime_count);
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
    public void PrintOptions(Dictionary<int, string> dict_saved_selector, int anime_count)
    {
        List<string> list_option_2 =  new List<string>();
        List<int> user_choice_on_list = new List<int>();

        while (true)
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
                user_choice_on_list.Add(input);
                InDepthAnimeDescription(input, dict_saved_selector);
            }
            else if (user_input == "2")
            {
                Console.Write("Enter the genre(s) you would like to filter the list. If multiple, place a comma to separate them: ");
                string user_genre = Console.ReadLine()!;
                list_option_2.Add(user_genre);

                //var anime_choice = user_choice_on_list[0];
                //string anime_name = dict_saved_selector[anime_choice];       // Convert number back to anime title for genre_dict to use as a key. Issue here.

                // UserDefinedGenreList(list_option_2);
            }
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

        Anime anime_indepth = new Anime();

        /*
            Print out:
            - Title
            - Synopsis
            - image url 
            - score
            - Popularity
            - members
            - Rank
            - Season 
            - studio 
            - Genre
            - Themes
            - Demographic
            - Source
            - Type (tv, ona, special, movie, etc)
         */
        foreach (var item in _anime_name_result["data"])
        {
            anime_indepth.Title = item["title"]!.ToString();
            anime_indepth.Synopsis = item["synopsis"]!.ToString();
            anime_indepth.Url = item["url"]!.ToString();
            anime_indepth.Img = item["images"]["jpg"]["image_url"].ToString();
            anime_indepth.Score = item["score"]!.ToString();
            anime_indepth.Score_By = item["scored_by"]!.ToString();
            anime_indepth.Rank = item["rank"]!.ToString();
            anime_indepth.Popularity = int.Parse(item["popularity"]!.ToString());
            anime_indepth.Members = item["members"]!.ToString();
            anime_indepth.Rank = item["rank"]!.ToString();
            anime_indepth.Season = item["season"]!.ToString();
            // anime_indepth.Studio = item["studios"][0]["name"].ToString();
            anime_indepth.Studio = FetchEmbeddedJsonAttribute("studios");
            // anime_indepth.Genres = item["genres"].ToString();
            anime_indepth.Genres = FetchEmbeddedJsonAttribute("genres");
            //anime_indepth.Themes = item["themes"].ToString();
            anime_indepth.Themes = FetchEmbeddedJsonAttribute("themes");
            anime_indepth.Demographic = item["demographics"].ToString();
            anime_indepth.Source = item["source"]!.ToString();
            anime_indepth.Type = item["type"]!.ToString();

            // Img, studio, genres, themes, demographic need an additional loop.
            //Console.WriteLine(item);
            Console.WriteLine($"Title: {anime_indepth.Title}");
            Console.WriteLine($"Synopsis: {anime_indepth.Synopsis}");
            Console.WriteLine($"URL: {anime_indepth.Url}");
            Console.WriteLine($"Img: {anime_indepth.Img}");
            Console.WriteLine($"Score: {anime_indepth.Score}");
            Console.WriteLine($"Scored By: {anime_indepth.Score_By}");
            Console.WriteLine($"Rank: {anime_indepth.Rank}");
            Console.WriteLine($"Popularity: {anime_indepth.Popularity}");
            Console.WriteLine($"Members: {anime_indepth.Members}");
            Console.WriteLine($"Season: {anime_indepth.Season}");
            Console.WriteLine($"Studio: {anime_indepth.Studio}");
            Console.WriteLine($"Genres: {anime_indepth.Genres}");
            Console.WriteLine($"Themes: {anime_indepth.Themes}");
            Console.WriteLine($"Demographic: {anime_indepth.Demographic}");
            Console.WriteLine($"Source: {anime_indepth.Source}");
            Console.WriteLine($"Type: {anime_indepth.Type}");
        }
    }

    // Loops through the json value that have more than one value such as studio, genres, themes, etc. The only difference is one small word difference.
    // If the value is empty, just return "[]".
    public string FetchEmbeddedJsonAttribute(string attribute_name)
    {
        List<string> list_att_name = new List<string>();
        string value = "";

        foreach (var item in _anime_name_result["data"])
        {
            foreach (var x in item[attribute_name])
            {
                list_att_name.Add(x["name"].ToString());
            }
        }

        if (list_att_name.Count == 1)
        {
            value = list_att_name[0];
        }
        else
        {
            foreach (var item in list_att_name)
            {
                value += $"{item}, ";
            }
        }


        return value;
    }

    // Expand on number 2 menu option.
    public void UserDefinedGenreList(List<string> list_menu_2_choices)
    {
        
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