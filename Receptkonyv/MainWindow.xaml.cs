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

namespace Receptkonyv
{
   
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Kilepes_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Kategoriak_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Itt fog megnyílni a Kategóriák ablaka!", "Hamarosan...");
        }

        private void Receptek_Click(object sender, RoutedEventArgs e)
        {
            ReceptekAblak ablak = new ReceptekAblak();
            ablak.Owner = this;
            ablak.ShowDialog();
        }
    }
}