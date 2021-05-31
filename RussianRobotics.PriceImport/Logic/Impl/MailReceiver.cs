using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RussianRobotics.PriceImport.Logic
{
    /// <summary>Реализует интерфейс <see cref="IReceiver"/> получения входящих писем.</summary>
    public class MailReceiver : IReceiver
    {
        private readonly IImapClient imapClient;
        private readonly bool leaveOpen;
        private bool disposed;

        /// <summary>Инициализирует новый экземпляр класса <see cref="MailReceiver"/>.</summary>
        /// <param name="imapClient">Интерфейс для работы с IMAP.</param>
        /// <param name="leaveOpen">True если не требуется освободить ресурсы <see cref="IImapClient"/>, иначе false.</param>
        public MailReceiver(IImapClient imapClient, bool leaveOpen = true)
        {
            this.imapClient = imapClient;
            this.leaveOpen = leaveOpen;
        }

        /// <summary>Инициализирует новый экземпляр класса <see cref="MailReceiver"/>.</summary>
        public MailReceiver() : this(new ImapClient(), false) { }

        /// <summary><inheritdoc/></summary>
        public void Connect(string userName, string password, string host, int port, bool useSsl)
        {
            imapClient.Connect(host, port, useSsl);
            imapClient.Authenticate(userName, password);
        }

        /// <summary><inheritdoc/></summary>
        public IEnumerable<MimePart> FindAttchmentsByFileExtensions(MimeMessage message, params string[] fileExtensions)
        {
            return message.Attachments.Where(x => fileExtensions.Any(y => x.ContentDisposition.FileName.EndsWith($".{y}", StringComparison.OrdinalIgnoreCase)))
                                      .Select(x => x as MimePart);
        }

        /// <summary><inheritdoc/></summary>
        public IEnumerable<MimeMessage> FindMessagesBySender(string from)
        {
            IMailFolder inbox = imapClient.Inbox;
            inbox.Open(FolderAccess.ReadOnly);
            TextSearchQuery query = SearchQuery.FromContains(from);

            foreach (var uid in inbox.Search(query))
                yield return inbox.GetMessage(uid);
        }

        /// <summary><inheritdoc/></summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (!leaveOpen && disposing)
                {
                    imapClient?.Dispose();
                }

                disposed = true;
            }
        }        
    }
}
