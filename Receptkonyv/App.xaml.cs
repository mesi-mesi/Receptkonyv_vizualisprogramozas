using System.Collections.Generic;
using System.Linq;
using System.Windows;

// PROJEKT: Receptkönyv
// FORRÁS: Cn-EFC-MF-PhoneBookSimple (Program.cs főlogikája alapján)
// LOGIKA: Adatbázis inicializálás és kezdeti adatok (Seeding) betöltése listából.

namespace Receptkonyv
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            using (var db = new ReceptContext())
            {
                // Séma létrehozása, ha még nem létezik
                db.Database.EnsureCreated();

                // Kezdeti adatok betöltése listából, ha az adatbázis üres
                if (!db.Kategoriak.Any())
                {
                    // Kategóriák létrehozása
                    var levesek = new Kategoria { Megnevezes = "Levesek" };
                    var foetelek = new Kategoria { Megnevezes = "Főételek" };
                    var desszertek = new Kategoria { Megnevezes = "Desszertek" };

                    var kategoriaLista = new List<Kategoria> { levesek, foetelek, desszertek };
                    db.Kategoriak.AddRange(kategoriaLista);

                    //Recept és hozzávalók összekapcsolt mentése
                    var gulyas = new Recept
                    {
                        Cim = "Alföldi Gulyásleves",
                        ElkeszitesiIdo = 120,
                        Kategoria = levesek
                    };

                    gulyas.Hozzavalok.Add(new Hozzavalo { Nev = "Marhahús", Mennyiseg = "500 g" });
                    gulyas.Hozzavalok.Add(new Hozzavalo { Nev = "Burgonya", Mennyiseg = "3 db" });
                    gulyas.Hozzavalok.Add(new Hozzavalo { Nev = "Pirospaprika", Mennyiseg = "2 evőkanál" });

                    var palacsinta = new Recept
                    {
                        Cim = "Kakaós Palacsinta",
                        ElkeszitesiIdo = 30,
                        Kategoria = desszertek
                    };

                    palacsinta.Hozzavalok.Add(new Hozzavalo { Nev = "Liszt", Mennyiseg = "200 g" });
                    palacsinta.Hozzavalok.Add(new Hozzavalo { Nev = "Tej", Mennyiseg = "3 dl" });

                    var receptLista = new List<Recept> { gulyas, palacsinta };
                    db.Receptek.AddRange(receptLista);

                    //Változások mentése az SQL adatbázisba.
                    db.SaveChanges();
                }
            }
        }
    }
}
