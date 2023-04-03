using System;
using System.Threading.Tasks;
using Bot.Startup;

namespace Bot
{
    class Program
    {
        static void Main(string[] args)
        {
            var webServer = new WebServer();
            var bot = new BotSetup();
            Parallel.Invoke(
                () => bot.RunAsync().GetAwaiter().GetResult(),
                () => webServer.Task());
        }
    }
}