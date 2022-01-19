using System;
using System.Data;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace Discord_Bot.Commands
{
    [Group("cal")]
    [Description("A calculator that does math")]
    public class CalCommands : BaseCommandModule
    {
        [Command("simple")]
        [Description("Simple Calculations Ex:/*+-")]
        public async Task Cal(CommandContext ctx, [Description("Evaluates a Math Equation")] string Equation)
        {
            var dt = new DataTable();
            var v = dt.Compute(Equation, "");
            await ctx.Channel.SendMessageAsync(v.ToString()).ConfigureAwait(false);
        }

        [Command("pt")]
        [Description("Calculates Pythagorean Theorem 0= Unknown Variable")]
        public async Task PT(CommandContext ctx, [Description("Leg A")] double A, [Description("Leg B")] double B, [Description("Hypotenuse")] double C)
        {
            if (C == 0 && !(B == 0) && !(A == 0))
            {
                C = Math.Sqrt((A * A) + (B * B));
                await ctx.Channel.SendMessageAsync("C=" + C.ToString()).ConfigureAwait(false);
            }
            else if (B == 0 && !(C == 0) && !(A == 0))
            {
                B = Math.Sqrt((C * C) - (A * A));
                await ctx.Channel.SendMessageAsync("B= " + B.ToString()).ConfigureAwait(false);
            }
            else if (A == 0 && !(C == 0) && !(B == 0))
            {
                A = Math.Sqrt((C * C) - (B * B));
                await ctx.Channel.SendMessageAsync("A= " + A.ToString()).ConfigureAwait(false);
            }
            else
                await ctx.Channel.SendMessageAsync("More than one variable is unknown").ConfigureAwait(false);
        }
        [Command("roots")]
        [Description("Finds the roots of any number using any index rounded to 4 decimal places")]
        public async Task Roots(CommandContext ctx, [Description("The number of the root")]double index, [Description("The number that is getting rooted")]double radicand)
        {
            double Answer = Math.Round(Math.Pow(radicand, 1.0 / index), 4);
            if(index == 1)
                await ctx.Channel.SendMessageAsync("Cant do first roots");
            else if (index == 2)
                await ctx.Channel.SendMessageAsync($"\u221A{radicand} = {Answer}");
            else if (index == 3)
                await ctx.Channel.SendMessageAsync($"\u221B{radicand} = {Answer}");
            else if (index == 4)
                await ctx.Channel.SendMessageAsync($"\u221C{radicand} = {Answer}");
            else
                await ctx.Channel.SendMessageAsync($"{radicand} to the {index}th root is {Answer}");
        }
    }
}
