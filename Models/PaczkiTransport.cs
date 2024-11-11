using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Delivery.Models
{
    public class PaczkiTransport
    {
        [Key]
        [Column("ID_paczki_transport")]  // Map C# property to SQL column
        public int ID_paczki_transport { get; set; }

        [Column("ID_paczki")]  // Map C# property to SQL column
        public int ID_paczki { get; set; }
        public Magazyn Paczka { get; set; }

        [Column("ID_transport")]  // Map C# property to SQL column
        public int ID_transport { get; set; }
        public Transport Transport { get; set; }

        [Column("data_odbioru")]  // Map C# property to SQL column
        public DateTime DataOdbioru { get; set; }

        [Column("data_oddania")]  // Map C# property to SQL column
        public DateTime DataOddania { get; set; }
    }
}
