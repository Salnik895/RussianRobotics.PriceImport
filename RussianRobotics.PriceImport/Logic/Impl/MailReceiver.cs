using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RussianRobotics.PriceImport.Logic
{
    public class MailReceiver : IReceiver
    {
        private readonly IImapClient imapClient;
        private readonly bool leaveOpen;
        private bool disposed;

        public MailReceiver(IImapClient imapClient, bool leaveOpen = true)
        {
            this.imapClient = imapClient;
            this.leaveOpen = leaveOpen;
        }

        public MailReceiver() : this(new ImapClient(), false) { }

        public void Connect(string userName, string password, string host, int port, bool useSsl)
        {
            imapClient.Connect(host, port, useSsl);
            imapClient.Authenticate(userName, password);
        }

        public IEnumerable<MimePart> FindAttchmentsByFileExtensions(MimeMessage message, params string[] fileExtensions)
        {
            return message.Attachments.Where(x => fileExtensions.Any(y => x.ContentDisposition.FileName.EndsWith($".{y}", StringComparison.OrdinalIgnoreCase)))
                                      .Select(x => x as MimePart);
        }

        public IEnumerable<MimeMessage> FindMessagesBySender(string from)
        {
            IMailFolder inbox = imapClient.Inbox;
            inbox.Open(FolderAccess.ReadOnly);
            TextSearchQuery query = SearchQuery.FromContains(from);

            foreach (var uid in inbox.Search(query))
                yield return inbox.GetMessage(uid);
        }

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