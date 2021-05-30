using Microsoft.Extensions.Configuration;
using System;

namespace RussianRobotics.PriceImport.Logic
{
    public interface IAttachmentHandler : IDisposable
    {
        void Connect(string userName, string password);
        void Initialize(IConfiguration config);
        void HandleAttachments();
        public void HandleAttachments(string from);
    }
}