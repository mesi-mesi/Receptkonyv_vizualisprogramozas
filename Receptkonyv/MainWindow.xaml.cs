using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

// PROJEKT: Receptkönyv
// FORRÁS: WPF-CustomDialogBox_SimpleDataBinding (Ablakkezelés)
// LOGIKA: A főablak eseménykezelői, menürendszer és ablaknyitás.

namespace Receptkonyv
{
   
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Alkalmazás bezárása.
        private void Kilepes_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Kategoriak_Click(object sender, RoutedEventArgs e)
        {
            // A korábbi MessageBox helyett most már megnyitjuk az igazi ablakot
            KategoriakAblak ablak = new KategoriakAblak();
            ablak.Owner = this;
            ablak.ShowDialog();
        }

        // Új ablak példányosítása és modális (ShowDialog) megnyitása.
        private void Receptek_Click(object sender, RoutedEventArgs e)
        {
            ReceptekAblak ablak = new ReceptekAblak();
            ablak.Owner = this;
            ablak.ShowDialog();
        }
    }
}