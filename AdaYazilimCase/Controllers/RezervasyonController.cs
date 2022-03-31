using AdaYazilimCase.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace AdaYazilimCase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RezervasyonController : ControllerBase
    {
        [HttpPost("rezervasyon-yapabilir-mi")]
        public IActionResult RezervasyonYapilabilirMi(RezervasyonModel model)
        {
            var rezervasyonSonuc = new RezervasyonSonuc();
            if (model.KisilerFarkliVagonlaraYerlestirilebilir)
            {
                var vagonlar = model.Tren.Vagonlar;
                var yerlestirilecekKisiSayisi = model.RezervasyonYapilacakKisiSayisi;
                for (int i = 0; i < vagonlar.Count(); i++)
                {
                    if (yerlestirilecekKisiSayisi == 0)
                    {
                        break;
                    }

                    if (vagonlar[i].DoluKoltukAdet < vagonlar[i].Kapasite * 0.7)
                // Online rezervasyonlarda, bir vagonun doluluk kapasitesi % 70'i geçmemelidir. Yani vagon kapasitesi 100 ise ve 70 koltuk dolu ise, o vagona rezervasyon yapılamaz.
                      
                    {
                        var yerlesimAyrintisi = rezervasyonSonuc.YerlesimAyrintilari.FirstOrDefault(x => x.VagonAdi == vagonlar[i].Ad);
                        if (yerlesimAyrintisi != null)
                        {
                            yerlesimAyrintisi.KisiSayisi += 1;
                        }
                        else
                        {
                            rezervasyonSonuc.YerlesimAyrintilari.Add(new YerlesimAyrinti
                            {
                                VagonAdi = vagonlar[i].Ad,
                                KisiSayisi = 1
                            });
                        }
                        vagonlar[i].DoluKoltukAdet += 1;
                        yerlestirilecekKisiSayisi -= 1;
                        i--;
                    }
                }

                //var yerlestirilecekKisiSayisi = model.RezervasyonYapilacakKisiSayisi;
                //foreach (var vagon in model.Tren.Vagonlar)
                //{
                //    var dolulukOrani = vagon.Kapasite * 0.7;

                //    bool vagondaYerKaldiMi = true;

                //    int suankiKoltukSayisi = vagon.DoluKoltukAdet;
                //    while (vagondaYerKaldiMi)
                //    {

                //        if (yerlestirilecekKisiSayisi == 0)
                //        {
                //            break;
                //        }


                //        if (suankiKoltukSayisi < dolulukOrani)
                //        {
                //            var yerlesimAyrintisi = rezervasyonSonuc.YerlesimAyrintilari.FirstOrDefault(x => x.VagonAdi == vagon.Ad);
                //            if (yerlesimAyrintisi != null)
                //            {
                //                yerlesimAyrintisi.KisiSayisi += 1;
                //            }
                //            else
                //            {
                //                rezervasyonSonuc.YerlesimAyrintilari.Add(new YerlesimAyrinti
                //                {
                //                    VagonAdi = vagon.Ad,
                //                    KisiSayisi = 1
                //                });
                //            }
                //            suankiKoltukSayisi += 1;
                //            yerlestirilecekKisiSayisi -= 1;
                //        }
                //        else
                //        {
                //            vagondaYerKaldiMi = false;
                //        }
                //    }

                //}

            }
            else
            {
                foreach (var vagon in model.Tren.Vagonlar)
                {
                    if (vagon.DoluKoltukAdet + model.RezervasyonYapilacakKisiSayisi <= vagon.Kapasite * 0.7)
                    {
                        rezervasyonSonuc.YerlesimAyrintilari.Add(new YerlesimAyrinti
                        {
                            VagonAdi = vagon.Ad,
                            KisiSayisi = model.RezervasyonYapilacakKisiSayisi
                        });
                        break;
                    }
                }
            }

            var basariliMi = (rezervasyonSonuc.YerlesimAyrintilari.Sum(x => x.KisiSayisi)) == model.RezervasyonYapilacakKisiSayisi;

            if (basariliMi)
            {
                rezervasyonSonuc.RezervasyonYapilabilir = true;
                return Ok(rezervasyonSonuc);
            }
            else
            {
                rezervasyonSonuc.RezervasyonYapilabilir = false;
                rezervasyonSonuc.YerlesimAyrintilari = new List<YerlesimAyrinti>();
                return BadRequest(rezervasyonSonuc);
            }
        }
    }
}
