using Microsoft.EntityFrameworkCore;

namespace RussianRobotics.PriceImport.Model
{
    /// <summary>Контекст базы данных 'Запасные части'.</summary>
    public class SparePartsContext : DbContext
    {
        private readonly string connectionString;

        /// <summary>Инициализирует новый экземпляр класса <see cref="SparePartsContext"/>.</summary>
        /// <param name="connectionString">Строка подключения.</param>
        public SparePartsContext(string connectionString)
        {
            this.connectionString = connectionString;
            Database.EnsureCreated();
        }


        /// <summary>Таблица 'Прайс-лист'.</summary>
        public DbSet<PriceItem> PriceItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer(connectionString);
    }
}