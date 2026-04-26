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
    }
}