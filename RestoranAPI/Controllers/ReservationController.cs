using Microsoft.AspNetCore.Mvc;
using RestoranAPI.Models;
using RestoranAPI.Services;

namespace RestoranAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly ReservationService _service;

        public ReservationController(ReservationService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult RezervasyonOlustur(Reservation r)
        {
            string sonuc = _service.RezervasyonEkle(r);
            return Ok(sonuc);
        }

        [HttpGet]
        public IActionResult RezervasyonlariGetir()
        {
            var liste = _service.RezervasyonListele();
            return Ok(liste);
        }

        [HttpDelete("{id}")]
        public IActionResult RezervasyonSil(int id)
        {
            string sonuc = _service.RezervasyonSil(id);
            return Ok(sonuc);
        }

        [HttpGet("BosSaatler")]
        public IActionResult BosSaatleriGetir(int tableId, DateTime tarih)
        {
            var bosSaatler = _service.BosSaatleriHesapla(tableId, tarih);
            return Ok(bosSaatler);
        }
    }
}