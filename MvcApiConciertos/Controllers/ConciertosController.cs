using Microsoft.AspNetCore.Mvc;
using MvcApiConciertos.Models;
using MvcApiConciertos.Services;

namespace MvcApiConciertos.Controllers
{
    public class ConciertosController : Controller
    {
        private ServiceApiConcierto service;

        public ConciertosController(ServiceApiConcierto service)
        {
            this.service = service;
        }

        public async Task<IActionResult> Categorias()
        {
            List<CategoriaEvento> categorias = await this.service.GetCategoriaEventoAsync();
            return View(categorias);
        }

        public async Task<IActionResult> Eventos()
        {
            List<Eventos> eventos = await this.service.GetEventosAsync();
            return View(eventos);
        }

        public async Task<IActionResult> EventosPorCategoria(int id)
        {
            List<Eventos> eventos = await this.service.FindEventosPorCategoriaAsync(id);
            return View(eventos);
        }

        public async Task<IActionResult> Create()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Eventos evento)
        {
            await this.service.CreateEventoAsync(evento);
            return RedirectToAction("Eventos");
        }
    }
}
