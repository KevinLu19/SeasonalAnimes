using DisCatSharp.CommandsNext;
using DisCatSharp.CommandsNext.Attributes;
using DisCatSharp.Entities;


using SeasonalAnime.Gil;
using SeasonalAnime.src.Instagram;

namespace SeasonalAnime.src.Discord.Commands;
internal class Main	: BaseCommandModule
{
	private static string jikan_api_website = "https://api.jikan.moe/v4/";
	private FetchCurrentSeason _fetch_season_inst = new(jikan_api_website);     // FetchCurrentSeason.cs file.

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
		// Print the results as a embedded text to make it easier on the eyes.

		_fetch_season_inst.GetAllSeasons();
		_fetch_season_inst.FilterAnimes();

		var popular_anime = _fetch_season_inst.DiscordBotPrintPopularity();

		string response_heading = "Here are the sorted list from most popular to least popular according to MyAnimeList.";

		// Create Embedded message
		var embed = new DiscordEmbedBuilder()
		{
			Title = response_heading,
			Description = popular_anime,
			Color = DiscordColor.Aquamarine,
			Timestamp = DateTimeOffset.UtcNow
		};

		//await ctx.RespondAsync(response_heading + Environment.NewLine + Environment.NewLine + popular_anime);
		await ctx.Message.RespondAsync(embed: embed.Build());
	}

	[Command("list"), Description("Lists all the usable commands.")]
	public async Task ListCommand(CommandContext ctx)
	{
		string command_list = @"
			- season => Grabs and displays the current list of anime seasons. List will be from most popular to least popular.
			- catchrate => Gives you detailed percentage on the catch rate of an entered Pokemon and their level.		
			- gil => Receives 200 Gil on a daily basis. usage of Gil will be implemented in the future.
";

		var embed = new DiscordEmbedBuilder()
		{
			Title = "List of Usable Commands",
			Description = command_list,
			Color = DiscordColor.Azure,
			Timestamp = DateTimeOffset.UtcNow
		};

		await ctx.Message.RespondAsync(embed: embed.Build());
	}

	//[Command("daily"), Description("Obtains daily Gil.")]
	//public async Task DailyCommand(CommandContext ctx)
	//{
	//	Daily daily = new();
	//	DateTime date_time = DateTime.UtcNow;       // Storing the date and time when command has been called. Needs to be stored in a database.

	//	var check = daily.CheckTime(date_time);

	//	if (check == true)
	//	{
	//		daily.GiveGil();        // Receives 200 Gil.

	//		// Bot returns a embedded message.
	//		string title = @"Claimed Daily!";
	//		string description = "";
	//		var embed = BuildEmbedMessage(title, description, DiscordColor.White);

	//		await ctx.Message.RespondAsync(embed: embed.Build());
	//	}
	//	else
	//	{
	//		string title = @"You have already claimed your daily!";
	//		string description = "";

	//		var embed = BuildEmbedMessage(title, description, DiscordColor.Red);

	//		await ctx.Message.RespondAsync(embed: embed.Build());
	//	}

	//}

	[Command("gil"), Description ("Gives 200 gil every 24 hour period.")]
	public async Task Gil(CommandContext ctx)
	{
		Daily daily = new Daily();
		DateTime dt = DateTime.Today;

		DailyDatabase daily_db = new DailyDatabase();

		

		await ctx.Message.RespondAsync($"Daily Claimed! {daily} have been added to your balance.");
		await ctx.Message.RespondAsync($"{dt} was when daily was called.");
		
	}

	[Command("catchrate"), Description("Gives detailed percentages on the catch rate of an entered Pokemon and their level.")]
	public async Task PokemonCommand(CommandContext ctx, [RemainingText] string pokemon)
	{

	}


	[Command("insta"), Description("Searches the latest post of given user.")]
	public async Task InstaCommand(CommandContext ctx, [RemainingText] string username)
	{
		string example_user = "katerina.soria";

		Insta insta = new Insta(example_user);
	}

	public DiscordEmbedBuilder BuildEmbedMessage(string title, string description, DiscordColor color)
	{
		var embed = new DiscordEmbedBuilder()
		{
			Title = title,
			Description = description,
			Color = color,
			Timestamp = DateTimeOffset.UtcNow,
		};

		return embed;
	}
}
