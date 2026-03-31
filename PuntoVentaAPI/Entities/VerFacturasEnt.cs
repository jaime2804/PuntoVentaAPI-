using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PuntoVentaAPI.Entities
{
    public class VerFacturasEnt
    {

        public int IdFactura { get; set; }

        public string? Fecha { get; set; }

        public int Subtotal { get; set; }

        public int IVA { get; set; }

        public int Total { get; set; }

        public int IdCajero { get; set; }

        public string? NombreCajero { get; set; }


        public class VerFacturasRespuesta
        {
            public VerFacturasRespuesta()
            {
                Codigo = "1";
                Mensaje = string.Empty;
                Dato = null;
                Datos = null;
            }

            public string Codigo { get; set; }
            public string Mensaje { get; set; }
            public VerFacturasEnt? Dato { get; set; }
            public List<VerFacturasEnt>? Datos { get; set; }

        }
    }
}
