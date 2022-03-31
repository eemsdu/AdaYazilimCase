using System.Collections.Generic;

namespace AdaYazilimCase.Models
{
    public class RezervasyonSonuc
    {
        public bool RezervasyonYapilabilir { get; set; }
        public List<YerlesimAyrinti> YerlesimAyrintilari { get; set; } = new List<YerlesimAyrinti>();
    }
}
