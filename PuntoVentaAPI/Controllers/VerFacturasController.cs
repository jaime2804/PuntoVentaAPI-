using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PuntoVentaAPI.Entities;
using static PuntoVentaAPI.Entities.EmpleadoEnt;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using static PuntoVentaAPI.Entities.VerFacturasEnt;

namespace PuntoVentaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VerFacturasController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public VerFacturasController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [AllowAnonymous]
        [Route("ConsultarFacturas")]
        [HttpGet]
        public IActionResult ConsultarFacturas(DateTime fechaInicio, DateTime fechaFin)
        {
            VerFacturasRespuesta verFacturasRespuesta = new VerFacturasRespuesta();
            try
            {
                using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var parametros = new { FechaInicio = fechaInicio, FechaFin = fechaFin };
                    var resultadoBD = db.Query<VerFacturasEnt>("ConsultarFacturasPorFecha", parametros, commandType: CommandType.StoredProcedure).ToList();

                    if (resultadoBD == null || resultadoBD.Count == 0)
                    {
                        verFacturasRespuesta.Codigo = "-1";
                        verFacturasRespuesta.Mensaje = "No hay facturas registradas en el rango de fechas especificado.";
                    }
                    else
                    {
                        verFacturasRespuesta.Datos = resultadoBD;
                        verFacturasRespuesta.Codigo = "1";
                        verFacturasRespuesta.Mensaje = "Facturas consultadas con éxito.";
                    }
                    return Ok(verFacturasRespuesta); 
                }
            }
            catch (SqlException ex)
            {
                return StatusCode(500, new { message = "Error al consultar facturas en la base de datos.", error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocurrió un error inesperado al consultar facturas.", error = ex.Message });
            }
        }


    }
}
