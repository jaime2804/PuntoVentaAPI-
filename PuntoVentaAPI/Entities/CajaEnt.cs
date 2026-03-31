namespace PuntoVentaAPI.Entities
{
    public class CajaEnt
    {
        public int IdCaja { get; }
        public decimal MontoInicial { get; set; }
        public DateTime FechaCreacion { get;  }
        public decimal MontoActual { get;  }
        public string? Estado { get; }
    }


    public class CajaRespuesta
    {
        public CajaRespuesta()
        {
            Codigo = "1";
            Mensaje = string.Empty;
            Dato = null;
            Datos = null;
        }

        public string Codigo { get; set; }
        public string Mensaje { get; set; }
        public CajaEnt? Dato { get; set; }
        public List<CajaEnt>? Datos { get; set; }
        // Lista de movimientos asociados
        public List<CajaEnt> Movimientos { get; set; } = new List<CajaEnt>();
    }
}
