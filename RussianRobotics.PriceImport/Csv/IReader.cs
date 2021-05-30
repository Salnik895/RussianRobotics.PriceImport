using System;

namespace RussianRobotics.PriceImport.Csv
{
    public interface IReader : IDisposable
    {
        bool Read();
    }
}