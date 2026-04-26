using System.Collections.Generic;
using System.Windows;

// PROJEKT: Receptkönyv
// FORRÁSOK: WPF-CustomDialogBox_SimpleDataBinding, WPF-Adatkotes-Objektumlista
// LOGIKA: Az űrlap logikája, ami összeköti a kapott Recept objektumot a felülettel.

namespace Receptkonyv
{
    public partial class ReceptSzerkesztoAblak : Window
    {

        public Recept AktualisRecept { get; set; }

        public ReceptSzerkesztoAblak(Recept recept, List<Kategoria> kategoriak)
        {
            InitializeComponent();
            AktualisRecept = recept;

            //DataContext beállítása az adatkötéshez
            grFelulet.DataContext = AktualisRecept;

            // ComboBox tartalmának feltöltése a memóriából.
            cbKategoria.ItemsSource = kategoriak;
        }

        //  Mentés.
        private void Mentés_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}