using System.Data;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace Discord_Bot.Commands
{
    class MathCommands : BaseCommandModule
    {
        [Command("cal")]
        [Description("Calculator work in progress")]
        public async Task Cal(CommandContext ctx, string Equation)
        {
            var dt = new DataTable();
            var v = dt.Compute(Equation, "");
            await ctx.Channel.SendMessageAsync(v.ToString()).ConfigureAwait(false);
        }
    }
}
