using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PuntoVentaAPI.Entities;
using System.Data;
using System.Data.SqlClient;
using static PuntoVentaAPI.Entities.InventarioEnt;

namespace PuntoVentaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventarioController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public InventarioController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

      
        [AllowAnonymous]
        [Route("ConsultarInventario")]
        [HttpGet]
        public IActionResult ConsultarInventario()
        {
            InventarioRespuesta InventarioRespuesta = new InventarioRespuesta();
            try
            {
                using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var resultadoBD = db.Query<InventarioEnt>("ConsultarInventario", new { }, commandType: CommandType.StoredProcedure).ToList();

                    if (resultadoBD == null || resultadoBD.Count == 0)
                    {
                        InventarioRespuesta.Codigo = "-1";
                        InventarioRespuesta.Mensaje = "No hay Inventario registrado.";
                    }
                    else
                    {
                        InventarioRespuesta.Datos = resultadoBD;
                        InventarioRespuesta.Codigo = "1";
                        InventarioRespuesta.Mensaje = "Inventario consultado con éxito.";
                    }
                    return Ok(InventarioRespuesta);
                }
            }
            catch (SqlException ex)
            {
               
                return StatusCode(500, new { message = "Error al consultar Inventario en la base de datos.", error = ex.Message });
            }
            catch (Exception ex)
            {
               
                return StatusCode(500, new { message = "Ocurrió un error inesperado al consultar Inventario.", error = ex.Message });
            }
        }

    }
}


