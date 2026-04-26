using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration; 

// PROJEKT: Receptkönyv
// FORRÁSOK: Hivatalos projektkövetelmény (appsettings.json használata)
// LOGIKA: A kapcsolati sztring dinamikus beolvasása a kőbe vésett kód helyett.

namespace Receptkonyv
{
    public class ReceptContext : DbContext
    {
        public virtual DbSet<Kategoria> Kategoriak { get; set; }
        public virtual DbSet<Recept> Receptek { get; set; }
        public virtual DbSet<Hozzavalo> Hozzavalok { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Konfiguráció felépítése és beolvasása az appsettings.json fájlból
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                //A "DefaultConnection" nevű sztring kiolvasása a JSON-ből
                string connectionString = configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
    }
}
