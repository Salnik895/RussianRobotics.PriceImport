using Microsoft.Extensions.Configuration;
using RussianRobotics.PriceImport.Logic;
using System;

namespace RussianRobotics.PriceImport
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Введите логин: ");
            string userName = Console.ReadLine();

            Console.Write("Введите пароль: ");
            string password = ConsoleEx.ReadPassword();

            using (var handler = new MailAttachmentHandler())
            {
                IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json", true, true)
                                                                  .Build();
                handler.Initialize(config);

                Console.WriteLine("Подключение...");
                handler.Connect(userName, password);

                Console.WriteLine("Обработка...");
                handler.HandleAttachments();

                Console.WriteLine("Готово.");
            }

            Console.ReadKey();
        }
    }
}