namespace PuntoVentaAPI.Entities
{
    public class ProveedorEnt
    {
     
            public long IdProveedor { get; set; }
            public string NumeroDocumento { get; set; }
            public string Nombre { get; set; }
            public string Correo { get; set; }
            public string Direccion { get; set; }
            public string Telefono { get; set; }
            public decimal Impuesto { get; set; }
        
        public class ProveedorRespuesta
        {
            public ProveedorRespuesta()
            {
                Codigo = "1"; 
                Mensaje = string.Empty;
                Dato = null;
                Datos = null;
            }

            public string Codigo { get; set; }
            public string Mensaje { get; set; }
            public ProveedorEnt? Dato { get; set; }
            public List<ProveedorEnt>? Datos { get; set; }
        }
    }
}
