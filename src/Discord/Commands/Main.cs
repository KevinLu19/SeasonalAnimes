using DisCatSharp.CommandsNext;
using DisCatSharp.CommandsNext.Attributes;

using SeasonalAnime;

namespace SeasonalAnime.src.Discord.Commands;
internal class Main	: BaseCommandModule
{
	// Test commands
	[Command("ping"), Description("Test command")]
	public async Task PingAsync(CommandContext ctx)
	{
		await ctx.RespondAsync($"{ctx.User.Mention}, Pong!");
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

	[Command("season"), Description("Grabs the current list of anime seasons")]
	public async Task SeasonCommand(CommandContext ctx)
	{
		
	}

	[Command("list"), Description("Lists all the usable commands.")]
	public async Task ListCommand(CommandContext ctx)
	{
		string command_list = @"
			- season => Grabs and displays the current list of anime seasons.
		";

		await ctx.RespondAsync(command_list);
	}
	
}
