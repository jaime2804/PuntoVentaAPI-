using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PuntoVentaAPI.Entities
{
    public class UsuarioEnt
    {
        public int IdUsuario { get; set; }
        public string? Identificacion { get; set; }
        public string? Nombre { get; set; }
        public string? Correo { get; set; }
        public string? Contrasenna { get; set; }
        public string? Estado { get; set; }
        public string? Descripcion { get; set; }
        public int IdRol { get; set; }

        public string? Token { get; set; }



        public class UsuarioRespuesta
        {
            public UsuarioRespuesta()
            {
                Codigo = "1";
                Mensaje = string.Empty;
                Dato = null;
                Datos = null;
            }


            public string Codigo { get; set; }
            public string Mensaje { get; set; }
            public UsuarioEnt? Dato { get; set; }
            public List<UsuarioEnt>? Datos { get; set; }

        }
    }

}