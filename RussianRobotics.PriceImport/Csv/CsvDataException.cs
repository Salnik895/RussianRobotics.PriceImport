using System;

namespace RussianRobotics.PriceImport.Csv
{
    /// <summary>Представляет ошибку обработи CSV файла.</summary>
    public class CsvDataException : Exception
    {
        /// <summary>Инициализирует новый экземпляр класса <see cref="CsvDataException"/>.</summary>
        public CsvDataException() { }

        /// <summary>Инициализирует новый экземпляр класса <see cref="CsvDataException"/>.</summary>
        /// <param name="message">Сообщение об ошибке.</param>
        public CsvDataException(string message) : base(message) { }

        /// <summary>Инициализирует новый экземпляр класс <see cref="CsvDataException"/>.</summary>
        /// <param name="message">Сообщение об ошибке.</param>
        /// <param name="index">Индекс строки.</param>
        /// <param name="line">Данные строки.</param>
        public CsvDataException(string message, int index, string line = null)
            : base($"Неудалось обработать строку '{index}': {message}." 
                  + (!string.IsNullOrWhiteSpace(line) ? $" Данные строки: {line}" : "")) 
        { }
    }
}