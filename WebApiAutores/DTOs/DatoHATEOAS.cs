namespace WebApiAutores.DTOs
{
    public class DatoHATEOAS
    {
        public string Enlace { get; private set; }
        public string Descripcion { get; private set; }
        public string Metodo { get; private set; }

        public DatoHATEOAS(string enlace, string descripcion, string metodo)
        {
            this.Enlace = enlace;
            this.Descripcion = descripcion;
            this.Metodo = metodo;
        }
    }
}
