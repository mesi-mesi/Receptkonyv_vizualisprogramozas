using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;

// PROJEKT: Receptkönyv
// FORRÁSOK: Cn-LINQ_OfType, WPF-DataBinding-DataGrid-Auto, Cn-EFC-MF-PhoneBookSimple, WPF-CustomDialogBox_SimpleDataBinding
// LOGIKA: Adatlekérés (Include), LINQ szűrés (Where), és adatbázis műveletek (CRUD).

namespace Receptkonyv
{
    public partial class ReceptekAblak : Window
    {
        private ReceptContext db;

        public ReceptekAblak()
        {
            InitializeComponent();
            db = new ReceptContext();
            AdatokBetoltese();
        }

        // Adatok betöltése.
        private void AdatokBetoltese()
        {
            dgReceptek.ItemsSource = db.Receptek.Include(r => r.Kategoria).ToList();
        }

        // Szűrés a memóriában lévő listán.
        private void tbKereso_TextChanged(object sender, TextChangedEventArgs e)
        {
            string keresoSzo = tbKereso.Text.ToLower();

            var szurtLista = db.Receptek.Include(r => r.Kategoria)
                                        .Where(r => r.Cim.ToLower().Contains(keresoSzo))
                                        .ToList();

            dgReceptek.ItemsSource = szurtLista;
        }

        // Új objektum felivtele az eadatbázisba.
        private void btUj_Click(object sender, RoutedEventArgs e)
        {
            var ujRecept = new Recept();
            var kategoriak = db.Kategoriak.ToList();

            var ablak = new ReceptSzerkesztoAblak(ujRecept, kategoriak) { Owner = this };

            if (ablak.ShowDialog() == true)
            {
                db.Receptek.Add(ablak.AktualisRecept);
                db.SaveChanges();
                AdatokBetoltese();
            }
        }

        //Adatmódosítás.
        private void btModosit_Click(object sender, RoutedEventArgs e)
        {
            if (dgReceptek.SelectedItem is Recept kivalasztottRecept)
            {
                var kategoriak = db.Kategoriak.ToList();
                var ablak = new ReceptSzerkesztoAblak(kivalasztottRecept, kategoriak) { Owner = this };

                if (ablak.ShowDialog() == true)
                {
                    db.SaveChanges();
                    AdatokBetoltese();
                }
            }
            else
            {
                MessageBox.Show("Kérlek, válassz ki egy receptet a módosításhoz!");
            }
        }

        //Adat törlése
        private void btTorol_Click(object sender, RoutedEventArgs e)
        {
            if (dgReceptek.SelectedItem is Recept kivalasztottRecept)
            {
                var valasz = MessageBox.Show($"Biztosan törlöd a(z) '{kivalasztottRecept.Cim}' receptet?",
                                             "Törlés", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (valasz == MessageBoxResult.Yes)
                {
                    db.Receptek.Remove(kivalasztottRecept);
                    db.SaveChanges();
                    AdatokBetoltese();
                }
            }
        }

        //Leválasztott (Detached) állapot generálása lekérdezéskor.
        // A kiválasztott recept hozzávalóinak megjelenítése
        // A kiválasztott recept hozzávalóinak megjelenítése

        // A kiválasztott recept hozzávalóinak megjelenítése
        private void btReszletek_Click(object sender, RoutedEventArgs e)
        {
            // 1. Ellenőrizzük, hogy ki van-e jelölve egy recept a táblázatban
            if (dgReceptek.SelectedItem is Recept kivalasztott)
            {
                // 2. Lekérdezzük a receptet az adatbázisból az ID alapján,
                // és az .Include() segítségével "beemeljük" a hozzá tartozó Hozzávalók listáját is.
                var receptHozzavalokkal = db.Receptek
                                            .Include(r => r.Hozzavalok)
                                            .FirstOrDefault(r => r.Id == kivalasztott.Id);

                if (receptHozzavalokkal != null)
                {
                    // 3. Felépítjük az üzenetablak tartalmát
                    string uzenet = $"=== {receptHozzavalokkal.Cim.ToUpper()} ===\n\n";

                    if (receptHozzavalokkal.Hozzavalok != null && receptHozzavalokkal.Hozzavalok.Any())
                    {
                        uzenet += "Szükséges hozzávalók:\n";
                        uzenet += "----------------------------------\n";
                        foreach (var h in receptHozzavalokkal.Hozzavalok)
                        {
                            uzenet += $"• {h.Nev}: {h.Mennyiseg}\n";
                        }
                    }
                    else
                    {
                        uzenet += "Ehhez a recepthez még nem rögzítettél hozzávalókat.\n";
                        uzenet += "Használd a '+ Hozzávaló' gombot a bővítéshez!";
                    }

                    // 4. Megjelenítés egy ízléses MessageBox-ban
                    MessageBox.Show(uzenet, "Recept Részletei", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Kérlek, előbb válassz ki egy receptet a listából!", "Nincs kijelölés", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Új hozzávaló hozzáadása a kiválasztott recepthez
        private void btUjHozzavalo_Click(object sender, RoutedEventArgs e)
        {
            if (dgReceptek.SelectedItem is Recept kivalasztottRecept)
            {
                // Megnyitjuk a kis felugró ablakot
                UjHozzavaloAblak ablak = new UjHozzavaloAblak();
                ablak.Owner = this;

                if (ablak.ShowDialog() == true)
                {
                    // Létrehozzuk az új hozzávalót a beírt adatokból
                    var ujHozzavalo = new Hozzavalo
                    {
                        Nev = ablak.UjNev,
                        Mennyiseg = ablak.UjMennyiseg,
                        ReceptId = kivalasztottRecept.Id // Összekötjük a kiválasztott recepttel (Külső kulcs!)
                    };

                    // Elmentjük az adatbázisba
                    db.Hozzavalok.Add(ujHozzavalo);
                    db.SaveChanges();

                    MessageBox.Show("A hozzávaló sikeresen rögzítve!", "Siker", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Kérlek, válassz ki egy receptet a listából, amihez a hozzávalót adod!", "Figyelem", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

    }
}