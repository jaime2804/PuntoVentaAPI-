namespace PuntoVentaAPI.Entities
{
    public class CategoriaEnt
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }

    public class CategoriaRespuesta
    {
        public CategoriaRespuesta()
        {
            Codigo = "1";
            Mensaje = string.Empty;
            Dato = null;
            Datos = null;
        }

        public string Codigo { get; set; }
        public string Mensaje { get; set; }
        public CategoriaEnt? Dato { get; set; }
        public List<CategoriaEnt>? Datos { get; set; }
    }
}
