using System;

namespace RussianRobotics.PriceImport.Csv
{
    /// <summary>Предоставляет методы для чтения данных из CSV файла.</summary>
    public interface IReader : IDisposable
    {
        /// <summary>Выполняет чтение записи.</summary>
        /// <returns>True если есть еще записи, иначе false.</returns>
        bool Read();
    }
}