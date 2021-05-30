using System.Globalization;
using System.Text.RegularExpressions;

namespace RussianRobotics.PriceImport.Model
{
    public class PriceItemBuilder
    {
        private static readonly Regex countRegex = new Regex(@"[0-9]+", RegexOptions.Compiled);
        private static readonly Regex searchRegex = new Regex(@"[^a-zA-Z0-9]+", RegexOptions.Compiled);
        private PriceItem record;

        public PriceItemBuilder() => record = new PriceItem();

        public PriceItemBuilder SetVendor(string vendor)
        {
            record.Vendor = vendor;
            record.SearchVendor = searchRegex.Replace(vendor, "").ToUpper();
            return this;
        }

        public PriceItemBuilder SetNumber(string number)
        {
            record.Number = number;
            record.SearchNumber = searchRegex.Replace(number, "").ToUpper();
            return this;
        }

        public PriceItemBuilder SetDescription(string description)
        {
            record.Description = description.Length > 512 ? description.Substring(0, 512) : description;
            return this;
        }

        public PriceItemBuilder SetPrice(string priceStr, CultureInfo culture)
        {
            record.Price = decimal.TryParse(priceStr, NumberStyles.Any, culture, out decimal price) ? price : default;            
            return this;
        }

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

        public PriceItem Build() => record;
    }
}