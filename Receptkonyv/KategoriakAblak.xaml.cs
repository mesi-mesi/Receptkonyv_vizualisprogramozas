// ****************************************************************************
// PROJEKT: Receptkönyv
// FORRÁSOK: Hivatalos projektkövetelmény (5. és 6. Funkcionalitás)
// LOGIKA: Kategóriák listázása, szűrése és új kategória felvitele.
// ****************************************************************************
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Receptkonyv
{
    public partial class KategoriakAblak : Window
    {
        private ReceptContext db;

        public KategoriakAblak()
        {
            InitializeComponent();
            db = new ReceptContext();
            AdatokBetoltese();
        }

        private void AdatokBetoltese()
        {
            dgKategoriak.ItemsSource = db.Kategoriak.ToList();
        }

        private void tbKereso_TextChanged(object sender, TextChangedEventArgs e)
        {
            string keresoSzo = tbKereso.Text.ToLower();

            var szurtLista = db.Kategoriak
                               .Where(k => k.Megnevezes.ToLower().Contains(keresoSzo))
                               .ToList();

            dgKategoriak.ItemsSource = szurtLista;
        }

        private void btUj_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(tbUjKategoria.Text))
            {
                var uj = new Kategoria { Megnevezes = tbUjKategoria.Text };
                db.Kategoriak.Add(uj);
                db.SaveChanges();

                AdatokBetoltese();
                tbUjKategoria.Text = "";
            }
            else
            {
                MessageBox.Show("Kérlek add meg az új kategória nevét!");
            }
        }

        // Kategória törlése feltétellel
        private void btTorol_Click(object sender, RoutedEventArgs e)
        {
            // 1. Ellenőrizzük, hogy van-e kijelölve kategória
            if (dgKategoriak.SelectedItem is Kategoria kivalasztott)
            {
                // 2. Ellenőrizzük az adatbázisban, hogy van-e hozzárendelt recept
                // (Megnézzük, létezik-e bármilyen recept ezzel a KategoriaId-val)
                bool vanHozzaadottRecept = db.Receptek.Any(r => r.KategoriaId == kivalasztott.Id);

                if (vanHozzaadottRecept)
                {
                    // Ha van recept, megtiltjuk a törlést
                    MessageBox.Show("Ez a kategória nem törölhető, mert receptek vannak hozzárendelve!",
                                    "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    // 3. Ha nincs recept, megerősítést kérünk és törlünk
                    var valasz = MessageBox.Show($"Biztosan törlöd a(z) '{kivalasztott.Megnevezes}' kategóriát?",
                                                 "Törlés megerősítése", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (valasz == MessageBoxResult.Yes)
                    {
                        db.Kategoriak.Remove(kivalasztott);
                        db.SaveChanges();
                        AdatokBetoltese();
                    }
                }
            }
            else
            {
                MessageBox.Show("Kérlek, válassz ki egy kategóriát a listából a törléshez!", "Figyelem", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}