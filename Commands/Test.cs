using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus;
using System.IO;
using Bot.Startup;
using System.Text.Json;

namespace Bot.Commands
{
    public class Test : BaseCommandModule
    {
        [Command("ping")]
        [Description("Returns Pong")]
        public async Task Ping(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("Pong").ConfigureAwait(false);
        }

        [Command("logchannel")]
        [Description("Warning this commands could break the bot, logs all messages to a text file in a channel")]
        [RequirePermissions(Permissions.Administrator)]
        public async Task LogChannel(CommandContext ctx, DiscordChannel channel)
        {
            var messages = await channel.GetMessagesAsync();

            using (StreamWriter writer = new StreamWriter("messages.txt"))
            {
                foreach (var message in messages)
                {
                    await writer.WriteLineAsync($"[{message.CreationTimestamp}] {message.Author.Username}: {message.Content}");
                }
            }

            await ctx.RespondAsync($"Logged {messages.Count} messages from #{channel.Name}.");
        }
    }
}
