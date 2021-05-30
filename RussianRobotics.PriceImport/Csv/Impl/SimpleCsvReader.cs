using System;
using System.Collections.Generic;
using System.IO;

namespace RussianRobotics.PriceImport.Csv
{
    public class SimpleCsvReader : IRowReader, IDisposable
    {
        private readonly IParser parser;
        private readonly Action<string[], int> badDataHandler;
        private readonly Dictionary<string, int> namedIndexes = new Dictionary<string, int>();
        private bool disposed;        
        private string[] header;
        
        public string[] Header => header;

        public SimpleCsvReader(IParser parser, Action<string[], int> badDataHandler)
        {
            this.parser = parser;
            this.badDataHandler = badDataHandler;
        }

        public SimpleCsvReader(TextReader textReader, 
                               Action<string[], int> badDataHandler = null, 
                               bool leaveOpen = true) 
            : this(new SimpleCsvParser(textReader, leaveOpen), badDataHandler) 
        { }

        public string GetField(int index)
        {
            if (index < parser.Record.Length)
                return parser.Record[index];
            else
                throw new IndexOutOfRangeException($"Столбец '{index}' не найден");
        }

        public string GetField(string name)
        {
            if (namedIndexes == null)
                throw new CsvDataException("Заголовок не был прочитан");

            if (namedIndexes.ContainsKey(name))
                return GetField(namedIndexes[name]);
            else
                throw new KeyNotFoundException($"Столбец '{name}' не найден");
        }

        public string[] GetRecord() => parser.Record;

        public bool Read()
        {
            bool flag = parser.Read();
            if (flag && header != null && header.Length != parser.Record.Length)
            {
                if (badDataHandler != null)
                {
                    badDataHandler.Invoke(parser.Record, parser.CurrentIndex);
                    parser.Read();
                }
                else
                    throw new CsvDataException("Размер строки не соответствует заголовку", parser.CurrentIndex);
            }
            return flag;
        }

        public bool ReadHeader()
        {
            if (parser.CurrentIndex == -1)
            {
                if (parser.Read())
                {
                    header = parser.Record;
                    for (int i = 0; i < header.Length; i++)
                        namedIndexes[header[i]] = i;
                    return true;
                }
            }

            return false;
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
                    parser?.Dispose();
                }

                disposed = true;
            }
        }
    }
}