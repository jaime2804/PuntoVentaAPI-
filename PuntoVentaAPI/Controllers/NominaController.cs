using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PuntoVentaAPI.Entities;
using PuntoVentaWeb.Entities;
using System.Data;
using System.Data.SqlClient;

namespace PuntoVentaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NominaController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public NominaController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpPost]
        [Route("CrearNomina")]
        public IActionResult CrearNomina(EmpleadoEnt empleado)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("Cedula", empleado.Cedula, DbType.Int32, ParameterDirection.Input);
                    parameters.Add("Nombre", empleado.Nombre, DbType.String, ParameterDirection.Input);
                    parameters.Add("Apellido", empleado.Apellido, DbType.String, ParameterDirection.Input);
                    parameters.Add("Vacaciones", empleado.Vacaciones, DbType.Int32, ParameterDirection.Input);
                    parameters.Add("Horas", empleado.Horas, DbType.Int32, ParameterDirection.Input);
                    parameters.Add("IdNomina", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    connection.Execute("CrearNomina", parameters, commandType: CommandType.StoredProcedure);
                    var idNomina = parameters.Get<int>("IdNomina");

                    return Ok(new { mensaje = "Nomina registrada con éxito", IdNomina = idNomina });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al registrar la nomina", error = ex.Message });
            }
        }


        [HttpGet]
        [Route("ObtenerNominas")]
        public IActionResult ObtenerNominas()
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var nominas = connection.Query<NominaEnt>("ObtenerNominas", commandType: CommandType.StoredProcedure).AsList();
                    return Ok(nominas);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al obtener las nominas", error = ex.Message });
            }
        }


    }
}
