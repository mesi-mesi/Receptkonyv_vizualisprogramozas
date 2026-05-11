

// PROJEKT: Receptkönyv
// FORRÁSOK: WPF-CustomDialogBox_SimpleDataBinding, WPF-Adatkotes-Objektumlista
// LOGIKA: Az űrlap logikája, ami összeköti a kapott Recept objektumot a felülettel,
//         valamint közvetlenül kezeli a recepthez tartozó hozzávalók CRUD műveleteit.
using System.Collections.Generic;
using System.Windows;

namespace Receptkonyv
{
    public partial class ReceptSzerkesztoAblak : Window
    {
        private ReceptContext db;
        public Recept AktualisRecept { get; set; }

        public ReceptSzerkesztoAblak(Recept recept, List<Kategoria> kategoriak, ReceptContext context)
        {
            InitializeComponent();
            AktualisRecept = recept;
            db = context;

            // DataContext beállítása az adatkötéshez
            grFelulet.DataContext = AktualisRecept;

            // ComboBox tartalmának feltöltése a memóriából
            cbKategoria.ItemsSource = kategoriak;

            // Hozzávalók megjelenítése
            HozzavalokListazasa();

            // === DINAMIKUS CÍM ÉS GOMBFELIRAT ===
            if (AktualisRecept.Id == 0)
            {
                Title = "Új Recept Felvitele";
                btMentes.Content = "Hozzáadás";
            }
            else
            {
                Title = "Recept Szerkesztése";
                btMentes.Content = "Mentés";
            }
        }

        // A DataGrid frissítése
        private void HozzavalokListazasa()
        {
            dgHozzavalok.ItemsSource = null;
            if (AktualisRecept.Hozzavalok != null)
            {
                dgHozzavalok.ItemsSource = AktualisRecept.Hozzavalok;
            }
        }

        // Új hozzávaló hozzáadása
        private void btHozzavaloUj_Click(object sender, RoutedEventArgs e)
        {
            var ablak = new UjHozzavaloAblak { Owner = this };
            if (ablak.ShowDialog() == true)
            {
                var ujHozzavalo = new Hozzavalo
                {
                    Nev = ablak.UjNev,
                    Mennyiseg = ablak.UjMennyiseg
                };

                AktualisRecept.Hozzavalok.Add(ujHozzavalo);
                HozzavalokListazasa();
            }
        }

        // Meglévő hozzávaló módosítása
        private void btHozzavaloModosit_Click(object sender, RoutedEventArgs e)
        {
            if (dgHozzavalok.SelectedItem is Hozzavalo kivalasztott)
            {
                var ablak = new UjHozzavaloAblak(kivalasztott.Nev, kivalasztott.Mennyiseg) { Owner = this };
                if (ablak.ShowDialog() == true)
                {
                    kivalasztott.Nev = ablak.UjNev;
                    kivalasztott.Mennyiseg = ablak.UjMennyiseg;
                    HozzavalokListazasa();
                }
            }
            else
            {
                MessageBox.Show("Kérlek, válassz ki egy hozzávalót a listából a módosításhoz!", "Figyelem", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Hozzávaló törlése
        private void btHozzavaloTorol_Click(object sender, RoutedEventArgs e)
        {
            if (dgHozzavalok.SelectedItem is Hozzavalo kivalasztott)
            {
                var valasz = MessageBox.Show($"Biztosan törlöd a(z) '{kivalasztott.Nev}' hozzávalót?",
                                             "Törlés megerősítése", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (valasz == MessageBoxResult.Yes)
                {
                    AktualisRecept.Hozzavalok.Remove(kivalasztott);

                    if (kivalasztott.Id > 0 && db != null)
                    {
                        db.Hozzavalok.Remove(kivalasztott);
                    }

                    HozzavalokListazasa();
                }
            }
            else
            {
                MessageBox.Show("Kérlek, válassz ki egy hozzávalót a törléshez!", "Figyelem", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Fő mentés gomb
        private void Mentés_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}