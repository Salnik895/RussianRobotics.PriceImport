using System;

namespace RussianRobotics.PriceImport
{
    /// <summary>Предоставляет статические методы расширяющие функционал консоли.</summary>
    public static class ConsoleEx
    {

        /// <summary>Выполняет чтение строки (пароля) с маскировкой отображаемых символов.</summary>
        public static string ReadPassword()
        {
            string password = string.Empty;
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

            Console.WriteLine();
            return password;
        }
    }
}
