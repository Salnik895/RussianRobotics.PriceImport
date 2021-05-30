using MimeKit;
using System;
using System.Collections.Generic;

namespace RussianRobotics.PriceImport.Logic
{
    public interface IReceiver : IDisposable
    {
        void Connect(string userName, string password, string host, int port, bool useSsl);
        IEnumerable<MimeMessage> FindMessagesBySender(string from);
        IEnumerable<MimePart> FindAttchmentsByFileExtensions(MimeMessage message, params string[] fileExtensions);
    }
}