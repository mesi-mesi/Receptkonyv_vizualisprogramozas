using System.Collections.Generic;
using System.Windows;

// PROJEKT: Receptkönyv 
// FORRÁS: Cn-EFC-MF-PhoneBookSimple (efPhoneBook.Person.cs minta alapján)
// LOGIKA: Az entitás alapú modellezés (POCO osztályok) és 1:N kapcsolatok.

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