using System;

namespace CoreConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Write your name");

            string userName;
            while (string.IsNullOrWhiteSpace(userName = Console.ReadLine()))
            {
                Console.WriteLine("Error! Please, write existing name.");
            }

            var welcomeHandler = new WelcomeHandler.WelcomeHandler();
            Console.WriteLine(welcomeHandler.WelcomeWrapper(userName));
            Console.Read();
        }
    }
}
