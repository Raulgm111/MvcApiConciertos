using Microsoft.AspNetCore.Mvc;
using MvcApiConciertos.Helpers;
using MvcApiConciertos.Models;
using MvcApiConciertos.Services;

namespace MvcApiConciertos.Controllers
{
    public class ConciertosController : Controller
    {
        private ServiceApiConcierto service;
        private ServiceStorageS3 serviceAWS;
        private string BucketUrl;

        public ConciertosController(ServiceApiConcierto service, ServiceStorageS3 serviceAWS, IConfiguration configuration)
        {
            this.service = service;
            this.BucketUrl = HelperSecretManager.GetSecretAsync("bucketexamen").Result;
            this.serviceAWS = serviceAWS;
        }

        public async Task<IActionResult> Categorias()
        {
            List<CategoriaEvento> categorias = await this.service.GetCategoriaEventoAsync();
            return View(categorias);
        }

        public async Task<IActionResult> Eventos()
        {
            List<Eventos> eventos = await this.service.GetEventosAsync();
            foreach (var evento in eventos)
            {
                using (Stream imageStream = await this.serviceAWS.GetFileAsync(evento.Imagen))
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    await imageStream.CopyToAsync(memoryStream);
                    byte[] bytes = memoryStream.ToArray();
                    evento.Imagen = Convert.ToBase64String(bytes);
                }
            }
            ViewData["Eventos"] = eventos;
            return View(eventos);
        }

        public async Task<IActionResult> EventosPorCategoria(int id)
        {
            List<Eventos> eventos = await this.service.FindEventosPorCategoriaAsync(id);
            foreach (var evento in eventos)
            {
                using (Stream imageStream = await this.serviceAWS.GetFileAsync(evento.Imagen))
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    await imageStream.CopyToAsync(memoryStream);
                    byte[] bytes = memoryStream.ToArray();
                    evento.Imagen = Convert.ToBase64String(bytes);
                }
            }
            ViewData["Eventos"] = eventos;
            return View(eventos);
        }

        public async Task<IActionResult> Create()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Eventos evento, IFormFile file)
        {
            using (Stream stream = file.OpenReadStream())
            {
                await this.serviceAWS.UploadFileAsync(file.FileName, stream);
            }
            await this.service.CreateEventoAsync(evento.Nombre,evento.Artista,evento.IdCategoria,file.FileName);
            return RedirectToAction("Eventos");
        }
    }
}
