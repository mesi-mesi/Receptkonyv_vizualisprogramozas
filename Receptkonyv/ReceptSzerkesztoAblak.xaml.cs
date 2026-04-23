using System.Collections.Generic;
using System.Windows;

namespace Receptkonyv
{
    public partial class ReceptSzerkesztoAblak : Window
    {
       
        public Recept AktualisRecept { get; set; }

        public ReceptSzerkesztoAblak(Recept recept, List<Kategoria> kategoriak)
        {
            InitializeComponent();
            AktualisRecept = recept;
           
            grFelulet.DataContext = AktualisRecept;

            cbKategoria.ItemsSource = kategoriak;
        }

            private void Mentés_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}