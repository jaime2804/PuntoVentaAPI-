namespace PuntoVentaAPI.Entities
{
        public class InventarioEnt
        {
            public int IdInventario { get; set; }
            public string IdProducto { get; set; }
            public string Nombre { get; set; }
            public decimal Precio { get; set; }
            public int Stock { get; set; }
            public int IdCategoria { get; set; }
            public DateTime FechaIngreso { get; set; }
        }


        public class InventarioRespuesta
        {
            public InventarioRespuesta()
            {
                Codigo = "1";
                Mensaje = string.Empty;
                Dato = null;
                Datos = null;
            }

            public string Codigo { get; set; }
            public string Mensaje { get; set; }
            public InventarioEnt? Dato { get; set; }
            public List<InventarioEnt>? Datos { get; set; }
        }
    }

