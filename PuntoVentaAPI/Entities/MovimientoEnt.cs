namespace PuntoVentaAPI.Entities
{
   
   

    public class MovimientoEnt
    {
        public int IdMovimiento { get; set; }
        public int IdCaja { get; set; }
        public string TipoMovimiento { get; set; }  // Ejemplo: "Ingreso" o "Egreso"
        public decimal Monto { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }
    }


    public class MovimientoRespuesta
    {
        public MovimientoRespuesta()
        {
            Codigo = "1";
            Mensaje = string.Empty;
            Dato = null;
            Datos = null;
        }

        public string Codigo { get; set; }
        public string Mensaje { get; set; }
        public MovimientoEnt? Dato { get; set; }
        public List<MovimientoEnt>? Datos { get; set; }
        // Lista de movimientos asociados
        public List<MovimientoEnt> Movimientos { get; set; } = new List<MovimientoEnt>();
    }
}
