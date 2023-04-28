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

        public static Dictionary<string, string> keyWords = new Dictionary<string, string>();
        public async Task RunAsync()
        {
            if (File.Exists("specificWords.json"))
            {
                string jsonString = File.ReadAllText("specificWords.json");
                keyWords = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString);
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

                string messageContent = e.Message.Content.ToLower();

                foreach (var word in keyWords)
                {
                    if (messageContent.Contains(word.Key))
                    {
                        if (word.Value.Contains("http"))
                            await e.Message.RespondAsync(word.Value);
                        else
                        {
                            var emoji = DiscordEmoji.FromName(Client, word.Value);
                            await e.Message.CreateReactionAsync(emoji);
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
