namespace RussianRobotics.PriceImport.Model
{
    /// <summary>Запись прайс-листа.</summary>
    public class PriceItem
    {
        /// <summary>Идентификатор.</summary>
        public int Id { get; set; }

        /// <summary>Бренд.</summary>
        public string Vendor { get; set; }

        /// <summary>Номер.</summary>
        public string Number { get; set; }

        public string SearchVendor { get; set; }

        public string SearchNumber { get; set; }

        /// <summary>Описание.</summary>
        public string Description { get; set; }

        /// <summary>Цена.</summary>
        public decimal Price { get; set; }

        /// <summary>Наличие (количество).</summary>
        public int Count { get; set; }
    }
}
