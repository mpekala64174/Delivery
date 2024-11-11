using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Delivery.Models
{
    public class Transport
    {
        public int ID_transport { get; set; }
        public int ID_uzytkownika { get; set; }
        public Uzytkownicy Uzytkownik { get; set; }
    }
}