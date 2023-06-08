using System.ComponentModel.DataAnnotations.Schema;

namespace MvcApiConciertos.Models
{
    public class Eventos
    {
        public string Nombre { get; set; }
        public string Artista { get; set; }
        public int IdCategoria { get; set; }
        public string Imagen { get; set; }
    }
}
