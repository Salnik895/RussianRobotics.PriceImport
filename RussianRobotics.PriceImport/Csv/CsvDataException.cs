using System;

namespace RussianRobotics.PriceImport.Csv
{
    public class CsvDataException : Exception
    {
        public CsvDataException() { }

        public CsvDataException(string message) : base(message) { }

        public CsvDataException(string message, int index, string line = null)
            : base($"Неудалось обработать строку '{index}': {message}." 
                  + (!string.IsNullOrWhiteSpace(line) ? $" Данные строки: {line}" : "")) 
        { }
    }
}