namespace PuntoVentaAPI.Entities
{
    public class ArqueoTotalesEnt
    {
        public decimal TotalBilletes { get; set; }
        public decimal TotalMonedas { get; set; }
        public decimal TotalEfectivo { get; set; }
    }

    public class ArqueoTotalesRespuesta
    {
        public ArqueoTotalesRespuesta()
        {
            Codigo = "1";
            Mensaje = string.Empty;
            Dato = null;
            Datos = null;
        }

        public string Codigo { get; set; }
        public string Mensaje { get; set; }
        public ArqueoTotalesEnt? Dato { get; set; }
        public List<ArqueoTotalesEnt>? Datos { get; set; }
    }
}