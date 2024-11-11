using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Delivery.Models
{
    [Table("paczki_automat")]  // Specify the table name
    public class PaczkiAutomat
    {
        [Key]
        [Column("ID_paczki_automat")]  // Map C# property to SQL column
        public int ID_paczki_automat { get; set; }

        [Column("ID_paczki_transport")]  // Map C# property to SQL column
        public int ID_paczki_transport { get; set; }
        public PaczkiTransport PaczkiTransport { get; set; }

        [Column("ID_automat")]  // Map C# property to SQL column
        public int ID_automat { get; set; }
        public AutomatPaczkowy AutomatPaczkowy { get; set; }
    }
}
