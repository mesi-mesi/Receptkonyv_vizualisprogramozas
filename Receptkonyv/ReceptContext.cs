using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

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
                // A kapcsolat a LocalDB-hez
                optionsBuilder.UseSqlServer(@"Data Source=(localdb)\mssqllocaldb;Initial Catalog=ReceptkonyvDB;Integrated Security=True;Persist Security Info=True");
            }
        }
    }
}
