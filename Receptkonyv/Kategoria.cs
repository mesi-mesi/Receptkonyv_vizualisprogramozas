using System.Collections.Generic;
using System.Windows;

namespace Receptkonyv
{
    public class Kategoria
    {
        public int Id { get; set; }
        public string Megnevezes { get; set; }

        // Kapcsolat: egy a többhöz kapcsolat
        public virtual ICollection<Recept> Receptek { get; set; } = new List<Recept>();
    }
}