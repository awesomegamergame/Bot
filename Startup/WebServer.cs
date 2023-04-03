using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Startup
{
    internal class WebServer
    {
        static Encoding enc = Encoding.UTF8;

        public Task Task()
        {
            Console.WriteLine("Started Http Server");
            var listener = new TcpListener(IPAddress.Any, 8080);
            listener.Start();

            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();

                NetworkStream stream = client.GetStream();
                string request = ToString(stream);

                StringBuilder builder = new StringBuilder();
                builder.AppendLine(@"HTTP/1.1 200 OK");
                builder.AppendLine(@"Content-Type: text/html");
                builder.AppendLine(@"");
                builder.AppendLine(@"<html><head><title>AGG Bot Webserver</title></head><body><h1>AGG Bot Webserver</h1>This Webserver keeps the discord bot online</body></html>");

                Console.WriteLine("Bot Pinged");

                byte[] sendBytes = enc.GetBytes(builder.ToString());
                stream.Write(sendBytes, 0, sendBytes.Length);

                stream.Close();
                client.Close();
            }
        }

        public static string ToString(NetworkStream stream)
        {
            MemoryStream memoryStream = new MemoryStream();
            byte[] data = new byte[256];
            int size;
            do
            {
                size = stream.Read(data, 0, data.Length);
                if (size == 0)
                {
                    Console.ReadLine();
                    return null;
                }
                memoryStream.Write(data, 0, size);
            } while (stream.DataAvailable);
            return enc.GetString(memoryStream.ToArray());
        }
    }
}
