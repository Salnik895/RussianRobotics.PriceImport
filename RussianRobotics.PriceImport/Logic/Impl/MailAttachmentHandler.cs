using Microsoft.Extensions.Configuration;
using MimeKit;
using RussianRobotics.PriceImport.Csv;
using RussianRobotics.PriceImport.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace RussianRobotics.PriceImport.Logic
{
    public class MailAttachmentHandler : IAttachmentHandler
    {
        private readonly IReceiver receiver;
        private readonly Dictionary<string, Dictionary<string, string>> fieldBindingsRegistry = new Dictionary<string, Dictionary<string, string>>();
        private bool disposed;
        private string connectionString;
        private string host;
        private int port;
        private bool useSsl;
        private CultureInfo culture;
        private string[] fileExtensions;
        
        public MailAttachmentHandler(IReceiver receiver) => this.receiver = receiver;

        public MailAttachmentHandler() : this(new MailReceiver()) { }

        public void Connect(string userName, string password)
            => receiver.Connect(userName, password, host, port, useSsl);

        public void HandleAttachments()
        {
            foreach (string from in fieldBindingsRegistry.Keys)
                HandleAttachments(from);
        }

        public void HandleAttachments(string from)
        {
            using (var dbContext = new SparePartsContext(connectionString))
            {
                foreach (MimeMessage message in receiver.FindMessagesBySender(from))
                    foreach (MimePart attachment in receiver.FindAttchmentsByFileExtensions(message, fileExtensions))
                        using (var stream = attachment.Content.Open())
                        using (var csv = new SimpleCsvReader(new StreamReader(stream), (r, i) => BadDataHadleCallback(from, attachment.FileName, r, i), false))
                        {
                            csv.ReadHeader();

                            List<string> header = csv.Header.ToList();
                            Dictionary<string, int> namedIndexes = header.ToDictionary(x => x, x => header.IndexOf(x));

                            while (csv.Read())
                            {
                                PriceItemBuilder priceRecordBuilder = new PriceItemBuilder();
                                PriceItem priceRecord = priceRecordBuilder.SetVendor(csv.GetField(fieldBindingsRegistry[from][nameof(PriceItem.Vendor)]))
                                                                          .SetNumber(csv.GetField(fieldBindingsRegistry[from][nameof(PriceItem.Number)]))
                                                                          .SetDescription(csv.GetField(fieldBindingsRegistry[from][nameof(PriceItem.Description)]))
                                                                          .SetPrice(csv.GetField(fieldBindingsRegistry[from][nameof(PriceItem.Price)]), culture)
                                                                          .SetCount(csv.GetField(fieldBindingsRegistry[from][nameof(PriceItem.Count)]))
                                                                          .Build();
                                dbContext.PriceItems.Add(priceRecord);
                            }

                            dbContext.SaveChanges();
                        }
            }
        }

        public void Initialize(IConfiguration config)
        {
            connectionString = config.GetSection("ConnectionString").Value;

            IConfigurationSection imapSettings = config.GetSection("ImapSettings");
            host = imapSettings.GetSection("Host").Value;
            port = int.Parse(imapSettings.GetSection("Port").Value);
            useSsl = bool.Parse(imapSettings.GetSection("UseSsl").Value);

            string cultureName = config.GetSection("Culture").Value;
            culture = !string.IsNullOrEmpty(cultureName) ? CultureInfo.GetCultureInfo(cultureName) : CultureInfo.CurrentCulture;

            IConfigurationSection attachmentsExtensions = config.GetSection("AttachmentsExtensions");
            fileExtensions = attachmentsExtensions.GetChildren().Select(x => x.Value).ToArray();

            IConfigurationSection fieldBindings = config.GetSection("FieldBindings");
            foreach (IConfigurationSection binding in fieldBindings.GetChildren())
            {
                IConfigurationSection fields = binding.GetSection("Fields");
                Dictionary<string, string> fieldsRegistry = new Dictionary<string, string>
                {
                    [nameof(PriceItem.Vendor)] = fields.GetSection(nameof(PriceItem.Vendor)).Value,
                    [nameof(PriceItem.Number)] = fields.GetSection(nameof(PriceItem.Number)).Value,
                    [nameof(PriceItem.Description)] = fields.GetSection(nameof(PriceItem.Description)).Value,
                    [nameof(PriceItem.Price)] = fields.GetSection(nameof(PriceItem.Price)).Value,
                    [nameof(PriceItem.Count)] = fields.GetSection(nameof(PriceItem.Count)).Value
                };
                fieldBindingsRegistry[binding.GetSection("PartnerMail").Value] = fieldsRegistry;
            }
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
                if (disposing)
                {
                    receiver?.Dispose();
                }

                disposed = true;
            }
        }

        private void BadDataHadleCallback(string from, string fileName, string[] record, int lineIndex)
            => Console.WriteLine($"Ошибка данных. Не удалось обработать строку. Отправитель: {from}. Файл: {fileName}. Строка: {lineIndex}.");
    }
}