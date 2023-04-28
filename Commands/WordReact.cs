using Bot.Startup;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Bot.Commands
{
    [GroupAttribute("wordreact")]
    public class WordReact : BaseCommandModule
    {
        [Command("add")]
        [Description("Add a new word reaction")]
        public async Task AddWordReaction(CommandContext ctx, string reactionWord, DiscordEmoji emoji)
        {
            string name = emoji.GetDiscordName();
            // Check if the emoji is already in the dictionary
            if (BotSetup.keyWords.ContainsKey(name))
            {
                // Add the word to the existing list for the emoji
                BotSetup.keyWords[name].Add(reactionWord.ToLower());
            }
            else
            {
                // Create a new list for the emoji and add the word
                BotSetup.keyWords[name] = new List<string>() { reactionWord.ToLower() };
            }

            // Save word reactions to file
            string jsonString = JsonSerializer.Serialize(BotSetup.keyWords);
            File.WriteAllText("wordReactions.json", jsonString);

            // Respond to the command with a success message
            await ctx.RespondAsync($"Added word reaction: {reactionWord} => {emoji}");
        }
        [Command("remove")]
        public async Task Remove(CommandContext ctx, string specificWord)
        {
            bool found = false;
            
            foreach (var reaction in BotSetup.keyWords)
            {
                if (reaction.Value.Contains(specificWord.ToLower()))
                {
                    reaction.Value.Remove(specificWord.ToLower());
                    found = true;

                    // Remove an emoji entry from the dictionary if it has no other words
                    if (reaction.Value.Count == 0)
                    {
                        BotSetup.keyWords.Remove(reaction.Key);
                    }

                }
            }

            if (!found)
            {
                await ctx.RespondAsync($"The word '{specificWord.ToLower()}' was not found in the dictionary.");
            }
            else
            {
                await ctx.RespondAsync($"The word '{specificWord.ToLower()}' was removed from the dictionary.");
                string jsonString = JsonSerializer.Serialize(BotSetup.keyWords);
                File.WriteAllText("wordReactions.json", jsonString);
            }
        }
        [Command("list")]
        public async Task List(CommandContext ctx)
        {
            string message = "List of emojis and their corresponding words:\n\n";

            foreach (var reaction in BotSetup.keyWords)
            {
                // Concatenate the associated words into a comma-separated string
                string words = string.Join(", ", reaction.Value);

                // Add the emoji and words to the message
                DiscordEmoji emoji = DiscordEmoji.FromName(ctx.Client, reaction.Key);
                message += $":{emoji}: {words}\n";
            }

            // Send the message to the channel
            await ctx.Channel.SendMessageAsync(message);
        }
    }
}
