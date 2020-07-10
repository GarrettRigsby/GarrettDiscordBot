/*
 * This class will handle any discord commands and pass them to the correct Command method.
 */

using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace GarrettBot.DiscordAPI
{
    public class CommandHandler
    {
        // Objects I'll need access to in this class.
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _client;
        private readonly IServiceProvider _services;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="services"></param>
        public CommandHandler(IServiceProvider services)
        {
            // Setup
            _commands = services.GetRequiredService<CommandService>();
            _client = services.GetRequiredService<DiscordSocketClient>();
            _services = services;

            // take action when we execute a command
            _commands.CommandExecuted += CommandExecutedAsync;

            // take action when we receive a message (so we can process it, and see if it is a valid command)
            _client.MessageReceived += MessageReceivedAsync;
        }

        /// <summary>
        /// Initialize and add reference to all the commands our bot will accept.
        /// </summary>
        /// <returns></returns>
        public async Task InitializeAsync()
        {
            // register modules that are public and inherit ModuleBase<T>.
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        /// <summary>
        /// When a message gets posted in discord, this event will trigger. 
        /// We will search to see if a matching command is found.
        /// </summary>
        /// <param name="rawMessage"></param>
        /// <returns></returns>
        public async Task MessageReceivedAsync(SocketMessage rawMessage)
        {
            // ensures we don't process system/other bot messages
            if (!(rawMessage is SocketUserMessage message) || message.Source != MessageSource.User)
                return;

            // sets the argument position away from the prefix we set
            int argPos = 0;

            // get prefix from the configuration file
            char prefix = '!';

            // determine if the message has a valid prefix, and adjust argPos based on prefix
            if (!(message.HasMentionPrefix(_client.CurrentUser, ref argPos) || message.HasCharPrefix(prefix, ref argPos)))
                return;

            // execute command if one is found that matches
            await _commands.ExecuteAsync(new SocketCommandContext(_client, message), argPos, _services);
        }

        /// <summary>
        /// This will execute the command and will notify the user of failures.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="context"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            // if a command isn't found, log that info to console and exit this method
            if (!command.IsSpecified)
            {
                Console.WriteLine("Command failed to execute!");
                return;
            }

            // log success to the console and exit this method
            if (result.IsSuccess)
            {
                Console.WriteLine("Command executed!");
                return;
            }

            // failure scenario, let's let the user know
            await context.Channel.SendMessageAsync("Something went wrong!");
        }
    }
}