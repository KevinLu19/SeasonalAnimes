// See https://aka.ms/new-console-template for more information
using DisCatSharp.Enums;
using DisCatSharp;
using SeasonalAnime;
using SeasonalAnime.src.Discord;
using Microsoft.Extensions.Configuration;
using DisCatSharp.CommandsNext;

/*
 Project goals: 
- Recommend to the user what anime to watch each current season. 
- Have a way to recommend to users what anime to watch for future season.
 
 */

// Seasonal Anime stuff
//const string jikan_api_website = "https://api.jikan.moe/v4/";

//FetchCurrentSeason fetch_anime = new FetchCurrentSeason(jikan_api_website);
//fetch_anime.GetAllSeasons();
//fetch_anime.FilterAnimes();

// ** Discord Bot **
/// <summary>
/// Bot Entry point.
/// </summary>
/// 
var con = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
var secret = con.Providers.First();
secret.TryGet("DISCORD__TOKEN", out var discord_token);

using var bot = new TifaDiscordBot(discord_token);
bot.RunAsync().Wait();


//MainAsync().GetAwaiter().GetResult();

//static async Task MainAsync()
//{
//	// Accessing user-secret that was set on local pc storage. 
//	// Ouputs discord varaible which contains the secret value from user-secrets.
//	var con = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
//	var secret = con.Providers.First();
//	secret.TryGet("DISCORD__TOKEN", out var discord_token);

//	var discord = new DiscordClient(new DiscordConfiguration()
//	{
//		Token = discord_token,
//		TokenType = TokenType.Bot,
//		Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContent
//	});

//	var command = discord.UseCommandsNext(new CommandsNextConfiguration()
//	{
//		StringPrefixes = new List<string>() { "!" }
//	});

//	command.RegisterCommands<SeasonalAnime.src.Discord.Commands.Main>();

//	await discord.ConnectAsync();
//	await Task.Delay(-1);
//}
