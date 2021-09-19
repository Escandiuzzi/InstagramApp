using System;

namespace Instagram_App
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--------------------------------- InstaManager ---------------------------------\n\n");

            string username = string.Empty;
            string password = string.Empty;
            string option = string.Empty;

            Console.WriteLine("Insert your username?");
            username = Console.ReadLine();

            Console.WriteLine("Insert your password?");

            password = string.Empty;
            ConsoleKey key;
            do
            {
                var keyInfo = Console.ReadKey(intercept: true);
                key = keyInfo.Key;

                if (key == ConsoleKey.Backspace && password.Length > 0)
                {
                    Console.Write("\b \b");
                    password = password[0..^1];
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    Console.Write("*");
                    password += keyInfo.KeyChar;
                }
            } while (key != ConsoleKey.Enter);


            Console.WriteLine("\nDo you want to remove verified users? Y/N");
            option = Console.ReadLine();

            InstagramManager instaManager = new InstagramManager(username, password, option.ToUpper() == "Y");
            Console.Read();
        }
    }
}
