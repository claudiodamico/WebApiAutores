using System.ComponentModel.DataAnnotations;

namespace WebApiAutores.DTOs
{
    public class LibroCreacionDTO
    {
        [Required]
        public string Titulo { get; set; }
        public DateTime FechaPublicaion { get; set; }
        public List<int> AutoresIds { get; set; }
    }
}
