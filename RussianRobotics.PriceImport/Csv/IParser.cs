namespace RussianRobotics.PriceImport.Csv
{
    /// <summary>Предоставляет методы и свойства для разбора CSV файла.</summary>
    public interface IParser : IReader
    {
        /// <summary>Индекс текущей строки.</summary>
        int CurrentIndex { get; }

        /// <summary>Данные текущей строки.</summary>
        string CurrentLine { get; }

        /// <summary>Значения полей текущей строки, представленной массивом.</summary>
        string[] Record { get; }
    }
}