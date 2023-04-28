using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.CommandsNext;
using Microsoft.Extensions.Logging;
using Bot.Commands;
using System.IO;
using System.Text.Json;
using DSharpPlus.Entities;

namespace Bot.Startup
{
    public class BotSetup
    {
        public string prefix = "!";
        public string token = Environment.GetEnvironmentVariable("Token");
        public DiscordClient Client { get; private set; }
        public InteractivityExtension Interactivity { get; private set; }
        public CommandsNextExtension Commands { get; private set; }

        public static Dictionary<string, List<string>> keyWords;
        public async Task RunAsync()
        {
            if (File.Exists("wordReactions.json"))
            {
                string jsonString = File.ReadAllText("wordReactions.json");
                keyWords = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(jsonString);
            }
            else
            {
                keyWords = new Dictionary<string, List<string>>();
            }

            var config = new DiscordConfiguration
            {
                Intents = DiscordIntents.All,
                Token = token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MinimumLogLevel = LogLevel.Debug
            };

            Client = new DiscordClient(config);

            Client.Ready += OnClientReady;

            Client.UseInteractivity(new InteractivityConfiguration
            {
                Timeout = TimeSpan.FromMinutes(2)
            });

            var commandsConfig = new CommandsNextConfiguration
            {
                StringPrefixes = new string[] { prefix },
                EnableDms = false,
                EnableMentionPrefix = true,
                DmHelp = false
            };

            Commands = Client.UseCommandsNext(commandsConfig);

            //Command classes should be registered here

            Commands.RegisterCommands<Test>();
            Commands.RegisterCommands<WordReact>();

            await Client.ConnectAsync();

            Client.MessageCreated += async (sender, e) =>
            {
                if (e.Message.Author.IsBot)
                    return;

                string message = e.Message.Content.ToLower();
                string[] messageContent = message.Split(' ');

                foreach (var reaction in keyWords)
                {
                    foreach (var word in reaction.Value)
                    {
                        if (messageContent.Contains(word))
                        {
                            // Send the corresponding emoji to the channel
                            await e.Message.CreateReactionAsync(DiscordEmoji.FromName(Client, reaction.Key));
                            break;
                        }
                    }
                }
            };

            await Task.Delay(-1);
        }

        private Task OnClientReady(DiscordClient sender, ReadyEventArgs e)
        {
            return Task.CompletedTask;
        }
    }
}
