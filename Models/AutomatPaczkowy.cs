using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Delivery.Models
{
    public class AutomatPaczkowy
    {
        [Key]
        [Column("ID_automat")]  // Map C# property to SQL column
        public int ID_automat { get; set; }

        [Column("lokalizacja")]  // Map C# property to SQL column
        public string Lokalizacja { get; set; }
    }
}
