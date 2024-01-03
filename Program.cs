// See https://aka.ms/new-console-template for more information
using SeasonalAnime;
/*
 Project goals: 
- Recommend to the user what anime to watch each current season. 
- Have a way to recommend to users what anime to watch for future season.
 
 */

const string jikan_api_website = "https://api.jikan.moe/v4/";

FetchCurrentSeason fetch_anime = new FetchCurrentSeason(jikan_api_website);
fetch_anime.GetAllSeasons();
fetch_anime.FilterAnimes();
