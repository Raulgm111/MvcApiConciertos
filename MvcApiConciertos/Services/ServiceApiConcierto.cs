using MvcApiConciertos.Helpers;
using MvcApiConciertos.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace MvcApiConciertos.Services
{
    public class ServiceApiConcierto
    {
        private MediaTypeWithQualityHeaderValue Header;
        private string UrlApi;


        public ServiceApiConcierto(IConfiguration configuration)
        {
            this.Header =
                new MediaTypeWithQualityHeaderValue("application/json");
            this.UrlApi =
                HelperSecretManager.GetSecretAsync("apimysql").Result;
        }

        private async Task<T> CallApiAsync<T>(string request)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                string url = this.UrlApi + request;
                HttpResponseMessage response =
                    await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }

        public async Task<List<CategoriaEvento>> GetCategoriaEventoAsync()
        {
            string request = "api/Conciertos/GetCategorias";
            List<CategoriaEvento> categorias = await this.CallApiAsync<List<CategoriaEvento>>(request);
            return categorias;
        }

        public async Task<List<Eventos>> GetEventosAsync()
        {
            string request = "api/Conciertos/GetEventos";
            List<Eventos> eventos = await this.CallApiAsync<List<Eventos>>(request);
            return eventos;
        }

        public async Task<List<Eventos>> FindEventosPorCategoriaAsync(int id)
        {
            string request = "api/Conciertos/"+id;
            List<Eventos> eventos = await this.CallApiAsync<List<Eventos>>(request);
            return eventos;
        }

        public async Task CreateEventoAsync(string nombre,string artista,int idcategoria,string imagen)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "api/Conciertos";
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                Eventos evento = new Eventos
                {
                    Nombre= nombre,
                    Artista= artista,
                    IdCategoria= idcategoria,
                    Imagen = imagen

                };
                string jsonComic = JsonConvert.SerializeObject(evento);
                StringContent content =
                    new StringContent(jsonComic, Encoding.UTF8, "application/json");
                HttpResponseMessage response =
                    await client.PostAsync(this.UrlApi + request, content);
            }
        }

    }
}
