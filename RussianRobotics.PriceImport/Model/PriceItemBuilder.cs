using System.Globalization;
using System.Text.RegularExpressions;

namespace RussianRobotics.PriceImport.Model
{
    /// <summary>Выполняет построение записи прайс-листа <see cref="PriceItem"/>.</summary>
    public class PriceItemBuilder
    {
        private static readonly Regex countRegex = new Regex(@"[0-9]+", RegexOptions.Compiled);
        private static readonly Regex searchRegex = new Regex(@"[^a-zA-Z0-9]+", RegexOptions.Compiled);
        private PriceItem record;

        /// <summary>Инициализирует новый экземпляр класса <see cref="PriceItemBuilder"/>.</summary>
        public PriceItemBuilder() => record = new PriceItem();


        /// <summary>Устанавливает бренд.</summary>
        public PriceItemBuilder SetVendor(string vendor)
        {
            record.Vendor = vendor;
            record.SearchVendor = searchRegex.Replace(vendor, "").ToUpper();
            return this;
        }

        /// <summary>Устанавливает номер.</summary>
        public PriceItemBuilder SetNumber(string number)
        {
            record.Number = number;
            record.SearchNumber = searchRegex.Replace(number, "").ToUpper();
            return this;
        }

        /// <summary>Устанавливает описание.</summary>
        public PriceItemBuilder SetDescription(string description)
        {
            record.Description = description.Length > 512 ? description.Substring(0, 512) : description;
            return this;
        }

        /// <summary>Устанавливает цену из указанной строки с указанным язык и региональным параметрам.</summary>
        public PriceItemBuilder SetPrice(string priceStr, CultureInfo culture)
        {
            record.Price = decimal.TryParse(priceStr, NumberStyles.Any, culture, out decimal price) ? price : default;            
            return this;
        }

        /// <summary>Устанавливает наличие (количество) из указанной строки.</summary>
        public PriceItemBuilder SetCount(string countStr)
        {
            int count = 0;
            int maxCount = int.MinValue;
            MatchCollection matches = countRegex.Matches(countStr);
            foreach (Match m in matches)
                if (int.TryParse(m.Value, out int mc) && mc > maxCount)
                    maxCount = mc;
            count = maxCount != int.MinValue ? maxCount : 0;

            record.Count = count;
            return this;
        }

        /// <summary>Возвращает экземпляр класса <see cref="PriceItem"/>.</summary>
        public PriceItem Build() => record;
    }
}
