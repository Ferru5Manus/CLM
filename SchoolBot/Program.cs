using System;

namespace SchoolBot
{
    class Program
    {
        static void Main(string[] args)
        {
            CLMBot bot = new CLMBot();  
            bot.RunAsync().GetAwaiter().GetResult();
        }
    }
}
