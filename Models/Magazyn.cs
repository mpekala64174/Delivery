using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Delivery.Models
{
    public class Magazyn
    {
        [Key]
        public int ID_paczki { get; set; }

        [Column("waga")]
        public decimal Waga { get; set; }

        [Column("rozmiar")]
        public string Rozmiar { get; set; }

        [Column("miejsce_odbioru")]
        public string MiejsceOdbioru { get; set; }

        [Column("status")]
        public string Status { get; set; }
    }


}
