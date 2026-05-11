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
            var lista = db.Receptek
                          .Include(r => r.Kategoria)
                          .Include(r => r.Hozzavalok)
                          .ToList();

            dgReceptek.ItemsSource = lista;

            // Automatikus kijelölés
            if (lista.Any())
            {
                dgReceptek.SelectedIndex = 0;
            }
            else
            {
                dgHozzavalok.ItemsSource = null;
            }
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

            if (szurtLista.Any())
            {
                dgReceptek.SelectedIndex = 0;
            }
            else
            {
                dgHozzavalok.ItemsSource = null;
            }
        }

        // === MASTER-DETAIL ESEMÉNYKEZELŐ ===
        private void dgReceptek_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgReceptek.SelectedItem is Recept kivalasztottRecept)
            {
                dgHozzavalok.ItemsSource = kivalasztottRecept.Hozzavalok;
            }
            else
            {
                dgHozzavalok.ItemsSource = null;
            }
        }

        // Új objektum felvitele
        private void btUj_Click(object sender, RoutedEventArgs e)
        {
            var ujRecept = new Recept();
            var kategoriak = db.Kategoriak.ToList();

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

                var ablak = new ReceptSzerkesztoAblak(kivalasztottRecept, kategoriak, db) { Owner = this };

                if (ablak.ShowDialog() == true)
                {
                    db.SaveChanges();
                    AdatokBetoltese();
                }
            }
            else
            {
                MessageBox.Show("Kérlek, válassz ki egy receptet a módosításhoz!", "Figyelem", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Adat törlése
        private void btTorol_Click(object sender, RoutedEventArgs e)
        {
            if (dgReceptek.SelectedItem is Recept kivalasztottRecept)
            {
                var valasz = MessageBox.Show($"Biztosan törlöd a(z) '{kivalasztottRecept.Cim}' receptet és a hozzá tartozó összes hozzávalót?",
                                             "Törlés megerősítése", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (valasz == MessageBoxResult.Yes)
                {
                    db.Receptek.Remove(kivalasztottRecept);
                    db.SaveChanges();
                    AdatokBetoltese();
                }
            }
            else
            {
                MessageBox.Show("Kérlek, válassz ki egy receptet a törléshez!", "Figyelem", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}