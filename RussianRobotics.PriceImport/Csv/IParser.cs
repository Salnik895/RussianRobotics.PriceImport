namespace RussianRobotics.PriceImport.Csv
{
    public interface IParser : IReader
    {
        int CurrentIndex { get; }
        string CurrentLine { get; }
        string[] Record { get; }
    }
}