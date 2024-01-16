using DisCatSharp;
using DisCatSharp.ApplicationCommands;
using DisCatSharp.CommandsNext;
using DisCatSharp.Entities;
using DisCatSharp.Enums;

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.Extensions.Logging;
using DisCatSharp.EventArgs;

namespace SeasonalAnime.src.Discord;
internal class TifaDiscordBot : IDisposable
{
	public static CancellationTokenSource ShutdownRequest;
	public static DiscordClient Client;

	private CommandsNextExtension _cNext;

	public TifaDiscordBot(string token)
	{
		ShutdownRequest = new();

		var cfg = new DiscordConfiguration
		{
			Token = token,
			TokenType = TokenType.Bot,
			Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContent
		};

		Client = new(cfg);

		this._cNext = Client.UseCommandsNext(new CommandsNextConfiguration()
		{
			StringPrefixes = new List<string>() { "!" },
			CaseSensitive = false
		});

		RegisterCommands(this._cNext);
	}


	/// <summary>
	/// Disposes the Bot.
	/// </summary>
	public void Dispose()
	{
		Client.Dispose();
		this._cNext = null;
		Client = null;
		Environment.Exit(0);
	}

	/// <summary>
	/// Starts the Bot.
	/// </summary>
	public async Task RunAsync()
	{
		await Client.ConnectAsync();
		await Task.Delay(-1);

		//await Client.ConnectAsync();
		//while (!ShutdownRequest.IsCancellationRequested)
		//	await Task.Delay(2000);
		//await Task.Delay(-1);
	}

	/// <summary>
	/// Registers the commands.
	/// </summary>
	private void RegisterCommands(CommandsNextExtension cnext)
	{
		cnext.RegisterCommands<src.Discord.Commands.Main>();    // src/discord/commands/main.cs (class) = commands file. 
	}


}