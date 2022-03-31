using System.Collections.Generic;

namespace AdaYazilimCase.Models
{
    public class Tren
    {
        public string Ad { get; set; }
        public List<Vagon> Vagonlar { get; set; } //Bir trende birden fazla vagon bulunabilir 
    }
}
