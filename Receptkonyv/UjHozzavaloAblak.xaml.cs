using System.Windows;

namespace Receptkonyv
{
    public partial class UjHozzavaloAblak : Window
    {
        public string UjNev { get; private set; }
        public string UjMennyiseg { get; private set; }

        public UjHozzavaloAblak()
        {
            InitializeComponent();
        }

        private void Hozzaadas_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(tbNev.Text) && !string.IsNullOrWhiteSpace(tbMennyiseg.Text))
            {
                UjNev = tbNev.Text;
                UjMennyiseg = tbMennyiseg.Text;
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Kérlek, töltsd ki mindkét mezőt!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}