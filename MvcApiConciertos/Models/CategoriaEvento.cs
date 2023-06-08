using System.ComponentModel.DataAnnotations.Schema;

namespace MvcApiConciertos.Models
{
    public class CategoriaEvento
    {
        public int IDCategoria { get; set; }
        public string Nombre { get; set; }
    }
}
