using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Delivery.Models
{
    public class Uzytkownicy
    {
        [Key]
        [Column("ID_uzytkownika")]  // Map C# property to SQL column
        public int ID_uzytkownika { get; set; }

        [Column("imie")]  // Map C# property to SQL column
        public string Imie { get; set; }

        [Column("nazwisko")]  // Map C# property to SQL column
        public string Nazwisko { get; set; }

        [Column("login")]  // Map C# property to SQL column
        public string Login { get; set; }

        [Column("haslo")]  // Map C# property to SQL column
        public string Haslo { get; set; }

        [Column("rola")]  // Map C# property to SQL column
        public string Rola { get; set; }

    }
}
