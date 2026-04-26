using System;
using System.Collections.Generic;
using System.Text;

// PROJEKT: Receptkönyv 
// FORRÁS: Cn-EFC-MF-PhoneBookSimple
// LOGIKA: Fő entitás (POCO) osztály definiálása, kapcsolatokkal.

namespace Receptkonyv
{
    public class Recept
    {
        public int Id { get; set; }
        public string Cim { get; set; }
        public int ElkeszitesiIdo { get; set; }

        // Külső kulcs 
        public int KategoriaId { get; set; }
        public virtual Kategoria Kategoria { get; set; }

        // Kapcsolat: egy a többhöz kapcsolat
        public virtual ICollection<Hozzavalo> Hozzavalok { get; set; } = new List<Hozzavalo>();
    }
}
