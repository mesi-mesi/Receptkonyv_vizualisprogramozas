using System;
using System.Collections.Generic;
using System.Text;

namespace Receptkonyv
{
    public class Hozzavalo
    {
        public int Id { get; set; }
        public string Nev { get; set; } 
        public string Mennyiseg { get; set; } 

        // Külső kulcs
        public int ReceptId { get; set; }
        public virtual Recept Recept { get; set; }
    }
}