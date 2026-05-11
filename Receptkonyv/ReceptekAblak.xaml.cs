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

        // Adatok betöltése a hozzávalókkal együtt (Include)
        private void AdatokBetoltese()
        {
            dgReceptek.ItemsSource = db.Receptek
                                       .Include(r => r.Kategoria)
                                       .Include(r => r.Hozzavalok)
                                       .ToList();
        }

        // Szűrés a memóriában lévő listán
        private void tbKereso_TextChanged(object sender, TextChangedEventArgs e)
        {
            string keresoSzo = tbKereso.Text.ToLower();

            var szurtLista = db.Receptek
                               .Include(r => r.Kategoria)
                               .Include(r => r.Hozzavalok)
                               .Where(r => r.Cim.ToLower().Contains(keresoSzo))
                               .ToList();

            dgReceptek.ItemsSource = szurtLista;
        }

        // Új objektum felvitele az adatbázisba
        private void btUj_Click(object sender, RoutedEventArgs e)
        {
            var ujRecept = new Recept();
            var kategoriak = db.Kategoriak.ToList();

            // Átadjuk a db kontextust is a hozzávalók kezeléséhez
            var ablak = new ReceptSzerkesztoAblak(ujRecept, kategoriak, db) { Owner = this };

            if (ablak.ShowDialog() == true)
            {
                db.Receptek.Add(ablak.AktualisRecept);
                db.SaveChanges();
                AdatokBetoltese();
            }
        }

        // Adatmódosítás és Hozzávalók kezelése
        private void btModosit_Click(object sender, RoutedEventArgs e)
        {
            if (dgReceptek.SelectedItem is Recept kivalasztottRecept)
            {
                var kategoriak = db.Kategoriak.ToList();

                // Átadjuk a db kontextust is a hozzávalók kezeléséhez
                var ablak = new ReceptSzerkesztoAblak(kivalasztottRecept, kategoriak, db) { Owner = this };

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

        // Adat törlése
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

        // A kiválasztott recept hozzávalóinak megjelenítése
        private void btReszletek_Click(object sender, RoutedEventArgs e)
        {
            if (dgReceptek.SelectedItem is Recept kivalasztott)
            {
                var receptHozzavalokkal = db.Receptek
                                            .Include(r => r.Hozzavalok)
                                            .FirstOrDefault(r => r.Id == kivalasztott.Id);

                if (receptHozzavalokkal != null)
                {
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
                        uzenet += "Használd a szerkesztőablakot a bővítéshez!";
                    }

                    MessageBox.Show(uzenet, "Recept Részletei", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Kérlek, előbb válassz ki egy receptet a listából!", "Nincs kijelölés", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}