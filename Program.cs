// See https://aka.ms/new-console-template for more information
using SeasonalAnime;
using RestSharp;
using System.Text.Json;
using System.Text.Json.Nodes;
using Newtonsoft.Json.Linq;

/*
 Project goals: 
- Recommend to the user what anime to watch each current season. 
- Have a way to recommend to users what anime to watch for future season.
 
 */

const string jikan_api_website = "https://api.jikan.moe/v4/";

FetchCurrentSeason fetch_anime = new FetchCurrentSeason(jikan_api_website);
fetch_anime.GetAllSeasons();
fetch_anime.FilterAnimes();


// Test
//var client = new RestClient(jikan_api_website);
//var request = new RestRequest("/seasons/now");
//var response = await client.GetAsync(request);

//var data = JsonSerializer.Deserialize<JsonNode>(response.Content!)!;

////Console.WriteLine(data["data"]);


//// Access Attribute inbedded dictionary values.
//JObject result = JObject.Parse(data.ToString());

//foreach (var item in result["data"])
//{
//	Console.WriteLine(item["mal_id"]);
//}
