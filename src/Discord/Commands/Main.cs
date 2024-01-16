using DisCatSharp.CommandsNext;
using DisCatSharp.CommandsNext.Attributes;

using SeasonalAnime;

namespace SeasonalAnime.src.Discord.Commands;
internal class Main	: BaseCommandModule
{
	private static string jikan_api_website = "https://api.jikan.moe/v4/";
	private FetchCurrentSeason _fetch_season_inst = new(jikan_api_website);		// FetchCurrentSeason.cs file.

	// Test commands
	[Command("ping"), Description("Test command")]
	public async Task PingAsync(CommandContext ctx)
	{
		await ctx.RespondAsync($"Pong!");
	}

	[Command("say"), Description("Says what you wrote.")]
	public async Task SayAsync(CommandContext ctx, [RemainingText] string msg)
	{
		await ctx.RespondAsync($"{ctx.User.Mention} said: {msg}");
	}

	[Command("greet"), Description("Greets the person.")]
	public async Task GreetCommand(CommandContext ctx)
	{
		await ctx.RespondAsync("Greetings! UwU");
	}

	[Command("anime"), Description("Grabs the current list of anime seasons")]
	public async Task AnimeCommand(CommandContext ctx)
	{
		_fetch_season_inst.GetAllSeasons();
		_fetch_season_inst.FilterAnimes();

		var popular_anime = _fetch_season_inst.DiscordBotPrintPopularity();

		string response_heading = "Here are the sorted list from most popular to least popular according to MyAnimeList. Keep in mind the list goes from most popular to least popular.";

		await ctx.RespondAsync(response_heading + Environment.NewLine + Environment.NewLine + popular_anime);
	}

	[Command("list"), Description("Lists all the usable commands.")]
	public async Task ListCommand(CommandContext ctx)
	{
		string command_list = @"
			* season => Displays the current sorted popular from most popular to least popular anime in the current season (Winter, Spring, Summer, Fall)
		";

		await ctx.RespondAsync(command_list);
	}

	
}
