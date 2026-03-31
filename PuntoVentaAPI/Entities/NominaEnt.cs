using System.ComponentModel;

namespace PuntoVentaWeb.Entities
{
    public class NominaEnt
    {
        [DisplayName("ID")]
        public long IdNomina { get; set; }

        [DisplayName("Cedula del Empleado")]
        public int Cedula { get; set; }

        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public int Vacaciones { get; set; }

        public int Horas { get; set; }

        [DisplayName("Salario Ajustado")]
        public int SalarioAjustado { get; set; }

        public class NominaRespuesta
        {
            public NominaRespuesta()
            {
                Codigo = "1";
                Mensaje = string.Empty;
                Dato = null;
                Datos = null;
            }

            public string Codigo { get; set; }
            public string Mensaje { get; set; }
            public NominaEnt? Dato { get; set; }
            public List<NominaEnt>? Datos { get; set; }
        }
    }
}