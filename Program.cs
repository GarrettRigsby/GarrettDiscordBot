/*
 * Author: Garrett Rigsby
 * Date: 07/10/2020
 * 
 * Learning how to make api calls and deserialize json.
 * Decided to go ahead and set it up as a DiscordBot and learn Discord commands/api as well.
 */

using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using GarrettBot.DiscordAPI;
using GarrettBot.BlizzardAPI;

namespace GarrettBot
{
    public class Program
    {
        // This is the connection to discord api
        private DiscordSocketClient _client;
        

        public static void Main(string[] args) 
            => new Program().MainAsync().GetAwaiter().GetResult();


        public async Task MainAsync()
        {
            // Init
            ServiceProvider services = new ServiceCollection()
                                        .AddSingleton<DiscordSocketClient>()
                                        .AddSingleton<CommandService>()
                                        .AddSingleton<CommandHandler>()
                                        .BuildServiceProvider();

            // Create Client
            _client = services.GetRequiredService<DiscordSocketClient>();

            // Events
            _client.Log += LogAsync;

            // Tokens
            string discordToken = Environment.GetEnvironmentVariable("DISCORDTOKEN");
            BlizzardAPIUtility.GetBlizzardAccessToken();

            // Connect to Discord
            await _client.LoginAsync(TokenType.Bot, discordToken);
            await _client.StartAsync();

            // CommandHandler
            await services.GetRequiredService<CommandHandler>().InitializeAsync();

            // block until program closed
            await Task.Delay(-1);
        }


        /// <summary>
        /// Async log.
        /// </summary>
        /// <param name="msg">Message to output.</param>
        /// <returns></returns>
        private Task LogAsync(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}