using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Discord_Bot.Startup
{
    class Decrypt
    {
        public static string Decrypter()
        {
            //string PasswordKey = Environment.GetEnvironmentVariable("Password"); //Repl.it's .env
            string PasswordKeyLocal = File.ReadAllText("password.txt"); //Local Password File
            //string textToDecrypt = File.ReadAllText("Protected.json"); //Stable File
            string textToDecrypt = File.ReadAllText("ProtectedNightly.json"); //Nightly File
            string ToReturn = "";
            string publickey = "agg-bot.86716543";
            string privatekey = PasswordKeyLocal; //Local Password File
            //string privatekey = PasswordKey; //Repl.it's .env
            byte[] privatekeyByte = { };
            privatekeyByte = Encoding.UTF8.GetBytes(privatekey);
            byte[] publickeybyte = { };
            publickeybyte = Encoding.UTF8.GetBytes(publickey);
            MemoryStream ms = null;
            CryptoStream cs = null;
            byte[] inputbyteArray = new byte[textToDecrypt.Replace(" ", "+").Length];
            inputbyteArray = Convert.FromBase64String(textToDecrypt.Replace(" ", "+"));
            using (var aes = Aes.Create("AesCryptoServiceProvider"))
            {
                ms = new MemoryStream();
                cs = new CryptoStream(ms, aes.CreateDecryptor(publickeybyte, privatekeyByte), CryptoStreamMode.Write);
                cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                cs.FlushFinalBlock();
                Encoding encoding = Encoding.UTF8;
                ToReturn = encoding.GetString(ms.ToArray());
            }
            Bot.Config = ToReturn;
            return ToReturn;
        }
    }
}
