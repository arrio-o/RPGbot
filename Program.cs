using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;

namespace RPGbot
{
    class Program
    {
        static void Main(string[] args)
        {
            var RPGbot = new DiscordClient();

            RPGbot.MessageReceived += (s, e) =>
            {
                if (!e.Message.IsAuthor)
                    Console.WriteLine(e.User.Name + " said " + e.Message.Text);
            };

            RPGbot.ExecuteAndWait(async () =>
            {
                await RPGbot.Connect("token");
            });
        }
    }
}
