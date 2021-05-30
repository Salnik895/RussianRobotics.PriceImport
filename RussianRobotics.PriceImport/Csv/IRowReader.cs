namespace RussianRobotics.PriceImport.Csv
{
    /// <summary>Предоставляет методы и свойства для чтения данных из CSV файла по строчно.</summary>
    public interface IRowReader : IReader
    {
        /// <summary>Заголовок таблицы.</summary>
        string[] Header { get; }

        /// <summary>Выполняет чтение заголовка таблицы.</summary>
        /// <returns>True если заголовок прочитан, иначе false (был прочитан ранее или в таблице отсутсвуют записи).</returns>
        bool ReadHeader();

        /// <summary>Возвращает строку.</summary>
        /// <returns>Массив значений полей строки.</returns>
        string[] GetRecord();

        /// <summary>Возвращает значения поля по индексу.</summary>
        /// <param name="index">Индекс поля.</param>
        /// <returns>Значение поля.</returns>
        string GetField(int index);

        /// <summary>Возвращает значения поля по имени.</summary>
        /// <param name="name">Имя поля.</param>
        /// <returns>Значение поля.</returns>
        string GetField(string name);
    }
}