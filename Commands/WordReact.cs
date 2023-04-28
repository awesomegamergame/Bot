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
        public async Task Add(CommandContext ctx, string specificWord, DiscordEmoji emoji)
        {
            BotSetup.keyWords[specificWord.ToLower()] = emoji.GetDiscordName();
            string jsonString = JsonSerializer.Serialize(BotSetup.keyWords);
            await File.WriteAllTextAsync("specificWords.json", jsonString);
            await ctx.RespondAsync($"'{specificWord}' has been set to '{emoji}'");
        }
        [Command("add")]
        public async Task Add(CommandContext ctx, string specificWord, string imageLink)
        {
            BotSetup.keyWords[specificWord.ToLower()] = imageLink;
            string jsonString = JsonSerializer.Serialize(BotSetup.keyWords);
            await File.WriteAllTextAsync("specificWords.json", jsonString);
            await ctx.RespondAsync($"'{specificWord}' has been set to '{imageLink}'");
        }
        [Command("remove")]
        public async Task Remove(CommandContext ctx, string specificWord)
        {
            if (BotSetup.keyWords.Remove(specificWord.ToLower()))
            {
                await ctx.RespondAsync($"'{specificWord}' has been removed");
                string jsonString = JsonSerializer.Serialize(BotSetup.keyWords);
                await File.WriteAllTextAsync("specificWords.json", jsonString);
            }
            else
            {
                await ctx.RespondAsync($"'{specificWord}' was not found in the list of specific words");
            }
        }
        [Command("list")]
        public async Task List(CommandContext ctx)
        {
            string message = "List of specific words and their corresponding response emojis:\n\n";

            foreach (var word in BotSetup.keyWords)
            {
                var emoji = DiscordEmoji.FromName(ctx.Client, word.Value);
                message += $"{word.Key} => {emoji}\n";
            }

            await ctx.RespondAsync(message);
        }
    }
}
