using MimeKit;
using System;
using System.Collections.Generic;

namespace RussianRobotics.PriceImport.Logic
{
    /// <summary>Предоставляет методы для работы с IMAP.</summary>
    public interface IReceiver : IDisposable
    {
        /// <summary>Выполняет подключение к почтовому ящику.</summary>
        /// <param name="userName">Имя пользователя.</param>
        /// <param name="password">Пароль.</param>
        /// <param name="host">Адрес сервера.</param>
        /// <param name="port">Порт.</param>
        /// <param name="useSsl">True для использования SSL, иначе false.</param>
        void Connect(string userName, string password, string host, int port, bool useSsl);

        /// <summary>Выполняет поиск входящих писем по указанному адресу отправителя.</summary>
        /// <param name="from">Адрес отправителя.</param>
        /// <returns>Список найденных писем.</returns>
        IEnumerable<MimeMessage> FindMessagesBySender(string from);

        /// <summary>Выполняет поиск прикрепленных файлов к письму по указанным расширениям.</summary>
        /// <param name="message">Экземпляр класса <see cref="MimeMessage"/>, представляющего письмо.</param>
        /// <param name="fileExtensions">Список расширений (без точки).</param>
        /// <returns>Список найденных файлов <see cref="MimePart"/>.</returns>
        IEnumerable<MimePart> FindAttchmentsByFileExtensions(MimeMessage message, params string[] fileExtensions);
    }
}