using System.ComponentModel.DataAnnotations;

namespace WebApiAutores.DTOs
{
    public class LibroPatchDTO
    {
        [Required]
        public string Titulo { get; set; }
        public DateTime FechaPublicaion { get; set; }
    }
}
