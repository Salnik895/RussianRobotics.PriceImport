using System;
using System.Collections.Generic;
using System.IO;

namespace RussianRobotics.PriceImport.Csv
{
    public class SimpleCsvParser : IParser, IDisposable
    {
        private readonly TextReader textReader;
        private readonly bool leaveOpen;
        private bool disposed;
        private int currentIndex = -1;
        private string currentLine;
        private string[] record;

        public int CurrentIndex => currentIndex;
        public string CurrentLine => currentLine;
        public string[] Record => record;

        public SimpleCsvParser(TextReader textReader, bool leaveOpen = true)
        {
            this.textReader = textReader;
            this.leaveOpen = leaveOpen;
        }

        public bool Read()
        {
            currentLine = textReader.ReadLine();

            if (currentLine != null)
            {
                List<string> fields = new List<string>();
                char lc = default;
                int li = -1;

                for (int i = 0; i < currentLine.Length; i++)
                {
                    char cc = currentLine[i];
                    switch (cc)
                    {
                        case ';':
                            switch (lc)
                            {
                                case '"':
                                    continue;

                                default:
                                    string value = currentLine[(li == -1 ? 0 : li + 1)..i];
                                    fields.Add(value);
                                    lc = cc;
                                    li = i;
                                    break;
                            }
                            break;

                        case '"':
                            switch (lc)
                            {
                                case '"':
                                    if (++i < currentLine.Length)
                                    {
                                        char nc = currentLine[i];
                                        switch (nc)
                                        {
                                            case ';':
                                                string value = currentLine[(li + 1)..(i - 1)];
                                                fields.Add(value);
                                                lc = nc;
                                                li = i;
                                                break;
                                        }
                                    }
                                    break;

                                default:
                                    lc = cc;
                                    li = i;
                                    break;
                            }
                            break;

                        default:
                            if ((i + 1) == currentLine.Length)
                            {
                                string value = currentLine[(li + 1)..i];
                                fields.Add(value);
                            }
                            break;
                    }
                }

                record = fields.ToArray();
                currentIndex++;
                return true;
            }
            else
            {
                record = null;
                currentIndex = -1;
                return false;
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
                if (!leaveOpen && disposing)
                {
                    textReader?.Dispose();
                }

                disposed = true;
            }
        }
    }
}