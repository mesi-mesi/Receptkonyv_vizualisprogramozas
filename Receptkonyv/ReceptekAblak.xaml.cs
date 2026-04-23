using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;

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

        private void AdatokBetoltese()
        {
            dgReceptek.ItemsSource = db.Receptek.Include(r => r.Kategoria).ToList();
        }

        private void tbKereso_TextChanged(object sender, TextChangedEventArgs e)
        {
            string keresoSzo = tbKereso.Text.ToLower();

            var szurtLista = db.Receptek.Include(r => r.Kategoria)
                                        .Where(r => r.Cim.ToLower().Contains(keresoSzo))
                                        .ToList();

            dgReceptek.ItemsSource = szurtLista;
        }

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

    }
}