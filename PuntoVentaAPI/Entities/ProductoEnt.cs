namespace PuntoVentaAPI.Entities
{
    public class ProductoEnt
    {
        
            public string? IdProducto { get; set; }
            public string? Nombre { get; set; }
            public decimal Precio { get; set; }
            public int Stock { get; set; }

         public int IdCategoria { get; set;}
        public string? NombreCategoria { get; }
    }

    public class ProductoRespuesta
    {
        public ProductoRespuesta()
        {
            Codigo = "1";
            Mensaje = string.Empty;
            Dato = null;
            Datos = null;
        }

        public string Codigo { get; set; }
        public string Mensaje { get; set; }
        public ProductoEnt? Dato { get; set; }
        public List<ProductoEnt>? Datos { get; set; }
    }
}

