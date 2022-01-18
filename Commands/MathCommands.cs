using System;
using System.Data;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace Discord_Bot.Commands
{
    [Description("A Calculator")]
    [Group] class CalCommands : BaseCommandModule
    {
        [Command("simple")]
        [GroupCommand]
        [Description("Simple Calculations Ex:/*+-")]
        public async Task Cal(CommandContext ctx, string Equation)
        {
            var dt = new DataTable();
            var v = dt.Compute(Equation, "");
            await ctx.Channel.SendMessageAsync(v.ToString()).ConfigureAwait(false);
        }
        class PythagTheor : BaseCommandModule
        {

            [Command("pt")]
            [GroupCommand]
            [Description("Calculates Pythagorean Theorem 0= Unknown Variable")]
            public async Task PT(CommandContext ctx, [Description("Leg A")]double A, [Description("Leg B")]double B, [Description("Hypotenuse")]double C)
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
        }
    }
}
