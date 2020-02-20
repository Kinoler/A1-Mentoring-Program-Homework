using System;
using WelcоmeHandler;

namespace CoreConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Error! Please, write your name.");
                return;
            }

            string userName = args[0];
            if (string.IsNullOrWhiteSpace(userName))
            {
                Console.WriteLine("Error! Please, write existing name.");
                return;
            }

            var welcomeHandler = new WelcomeHandler();
            Console.WriteLine(welcomeHandler.WelcomeWrapper(userName));
        }
    }
}
