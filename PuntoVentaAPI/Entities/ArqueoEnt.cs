namespace PuntoVentaAPI.Entities
{
    public class ArqueoEnt
    {
        public int IdArqueo { get; set; }
        public int IdCaja { get; set; }
        public int? Billetes1000 { get; set; }
        public int? Billetes2000 { get; set; }
        public int? Billetes5000 { get; set; }
        public int? Billetes10000 { get; set; }
        public int? Billetes20000 { get; set; }
        public int? Monedas5 { get; set; }
        public int? Monedas10 { get; set; }
        public int? Monedas25 { get; set; }
        public int? Monedas50 { get; set; }
        public int? Monedas100 { get; set; }
        public int? Monedas500 { get; set; }
        public DateTime Fecha { get; set; }




        public class ArqueoRespuesta
        {
            public ArqueoRespuesta()
            {
                Codigo = "1";
                Mensaje = string.Empty;
                Dato = null;
                Datos = null;
            }

            public string Codigo { get; set; }
            public string Mensaje { get; set; }
            public ArqueoEnt? Dato { get; set; }
            public List<ArqueoEnt>? Datos { get; set; }
        }

    }


}
