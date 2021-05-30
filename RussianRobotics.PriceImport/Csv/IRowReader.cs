namespace RussianRobotics.PriceImport.Csv
{
    public interface IRowReader : IReader
    {
        string[] Header { get; }
        bool ReadHeader();
        string[] GetRecord();
        string GetField(int index);
        string GetField(string name);
    }
}