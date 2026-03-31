namespace PuntoVentaAPI.Entities
{
    public class CierreSemanalEnt
    {
      
        public decimal TotalBilletesSemana { get; set; }
        public decimal TotalMonedasSemana { get; set; }
        public decimal TotalEfectivoSemana { get; set; }
    }



    public class CierreSemanalRespuesta
    {
            public CierreSemanalRespuesta()
            {
                Codigo = "1";
                Mensaje = string.Empty;
                Dato = null;
                Datos = null;
            }

            public string Codigo { get; set; }
            public string Mensaje { get; set; }
            public CierreSemanalEnt? Dato { get; set; }
            public List<CierreSemanalEnt>? Datos { get; set; }
        }

    }



