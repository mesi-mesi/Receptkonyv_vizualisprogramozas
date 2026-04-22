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

        private void btUj_Click(object sender, RoutedEventArgs e) { MessageBox.Show("Hamarosan!"); }
        private void btModosit_Click(object sender, RoutedEventArgs e) { MessageBox.Show("Hamarosan!"); }
        private void btTorol_Click(object sender, RoutedEventArgs e) { MessageBox.Show("Hamarosan!"); }
    }
}