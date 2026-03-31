using PuntoVentaAPI.Entities;
using System.ComponentModel;


namespace PuntoVentaAPI.Entities
{
    public class EmpleadoEnt
    {
        public int Cedula { get; set; }

        public string Nombre { get; set; }

        public string Apellido { get; set; }

        [DisplayName("Fecha de Ingreso")]
        public DateTime  FechaIngreso { get; set; }

        public int HorasTrabajadas { get; set; }

        [DisplayName("Horas Rebajadas")]
        public int HorasRebajadas { get; set; }

        [DisplayName("Valor por Hora")]
        public int ValorPorHora { get; set; }


        [DisplayName("Salario Ajustado")]
        public int SalarioAjustado { get; set; }

        public int Vacaciones { get; set; }

        public int Horas { get; set; }

        public class EmpleadoRespuesta
        {
            public EmpleadoRespuesta()
            {
                Codigo = "1";
                Mensaje = string.Empty;
                Dato = null;
                Datos = null;
            }

            public string Codigo { get; set; }
            public string Mensaje { get; set; }
            public EmpleadoEnt? Dato { get; set; }
            public List<EmpleadoEnt>? Datos { get; set; }
        }
    }
}

