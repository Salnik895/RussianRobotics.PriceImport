using Microsoft.EntityFrameworkCore;

namespace RussianRobotics.PriceImport.Model
{
    class SparePartsContext : DbContext
    {
        private readonly string connectionString;

        public SparePartsContext(string connectionString)
        {
            this.connectionString = connectionString;
            Database.EnsureCreated();
        }

        public DbSet<PriceItem> PriceItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer(connectionString);
    }
}