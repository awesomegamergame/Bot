using System.Threading.Tasks;
using Discord_Bot.Startup;

namespace Discord_Bot
{
    class Program
    {
        static void Main(string[] args)
        {
            Decrypt.Decrypter();
            var webServer = new WebServer();
            var bot = new Bot();
            Parallel.Invoke(
                () => bot.RunAsync().GetAwaiter().GetResult(),
                () => webServer.Task());
        }
	}
}
