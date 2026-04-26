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
        private void btReszletek_Click(object sender, RoutedEventArgs e)
        {
            if (dgReceptek.SelectedItem is Recept kivalasztott)
            {
                var levalasztottRecept = db.Receptek
                                           .AsNoTracking()
                                           .FirstOrDefault(r => r.Id == kivalasztott.Id);

                if (levalasztottRecept != null)
                {
                    MessageBox.Show($"Ez a recept most 'Detached' állapotban van!\n\n" +
                                    $"Név: {levalasztottRecept.Cim}\n" +
                                    $"Ha most átírnánk a kódban, a db.SaveChanges() nem mentené el.",
                                    "EF Core Tracking Demo");
                }
            }
        }

    }
}