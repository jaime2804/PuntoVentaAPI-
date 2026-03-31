using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PuntoVentaAPI.Entities;
using System.Data;
using System.Data.SqlClient;
using static PuntoVentaAPI.Entities.FacturaEnt;

namespace PuntoVentaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacturaController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public FacturaController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [AllowAnonymous]
        [Route("RegistrarFactura")]
        [HttpPost]
        public IActionResult RegistrarFactura(FacturaEnt Factura)
        {
            FacturaRespuesta FacturaRespuesta = new FacturaRespuesta();
            try
            {
                using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var parametros = new
                    {
                        Factura.IdProducto,
                        Factura.Cantidad,
                        Factura.IdCajero,
                        Factura.NuevaFactura,
                        Factura.Descuento,
                        Factura.Pago,
                        Factura.TipoPago
                    };

                    var result = db.Execute("RegistrarDetalleFactura", parametros, commandType: CommandType.StoredProcedure);
                    if (result > 0)
                    {
                        FacturaRespuesta.Codigo = "1";
                        FacturaRespuesta.Mensaje = "Factura registrado con éxito.";
                        return Ok(FacturaRespuesta);
                    }
                    else
                    {
                        FacturaRespuesta.Codigo = "-1";
                        FacturaRespuesta.Mensaje = "No se pudo registrar el Factura.";
                        return BadRequest(FacturaRespuesta);
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocurrió un error al registrar el Factura.", error = ex.Message });
            }
        }

        [AllowAnonymous]
        [Route("ConsultarFactura")]
        [HttpGet]
        public IActionResult ConsultarFactura()
        {
            FacturaRespuesta FacturaRespuesta = new FacturaRespuesta();
            try
            {
                using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var resultadoBD = db.Query<FacturaEnt>("ConsultarFacturas", new { }, commandType: CommandType.StoredProcedure).ToList();

                    if (resultadoBD == null || resultadoBD.Count == 0)
                    {
                        FacturaRespuesta.Codigo = "-1";
                        FacturaRespuesta.Mensaje = "No hay Factura registrados.";
                    }
                    else
                    {
                        FacturaRespuesta.Datos = resultadoBD;
                        FacturaRespuesta.Codigo = "1";
                        FacturaRespuesta.Mensaje = "Factura consultados con éxito.";
                    }
                    return Ok(FacturaRespuesta);
                }
            }
            catch (SqlException ex)
            {

                return StatusCode(500, new { message = "Error al consultar Factura en la base de datos.", error = ex.Message });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { message = "Ocurrió un error inesperado al consultar Factura.", error = ex.Message });
            }
        }
        [AllowAnonymous]
        [Route("ConsultarUltimaFactura")]
        [HttpGet]
        public IActionResult ConsultarUltimaFactura()
        {
            FacturaRespuesta FacturaRespuesta = new FacturaRespuesta();
            try
            {
                using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var resultadoBD = db.Query<FacturaEnt>("ConsultarUltimaFactura", new { }, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    if (resultadoBD == null )
                    {
                        FacturaRespuesta.Codigo = "-1";
                        FacturaRespuesta.Mensaje = "No hay Factura registrados.";
                    }
                    else
                    {
                        FacturaRespuesta.Dato = resultadoBD;
                        FacturaRespuesta.Codigo = "1";
                        FacturaRespuesta.Mensaje = "Factura consultados con éxito.";
                    }
                    return Ok(FacturaRespuesta);
                }
            }
            catch (SqlException ex)
            {

                return StatusCode(500, new { message = "Error al consultar Factura en la base de datos.", error = ex.Message });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { message = "Ocurrió un error inesperado al consultar Factura.", error = ex.Message });
            }
        }

        [AllowAnonymous]
        [Route("ConsultarUnaFactura")]
        [HttpGet]
        public IActionResult ConsultarUnaFactura(int IdFactura)
        {
            FacturaRespuesta FacturaRespuesta = new FacturaRespuesta();
            try
            {
                using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var result = db.Query<FacturaEnt>("ObtenerFacturaPorId",
                        new { IdFactura },
                        commandType: CommandType.StoredProcedure).ToList();

                    if (result == null)
                    {
                        FacturaRespuesta.Codigo = "-1";
                        FacturaRespuesta.Mensaje = "No hay Factura registrados.";
                    }
                    else
                    {
                        FacturaRespuesta.Datos = result;
                        FacturaRespuesta.Codigo = "1";
                        FacturaRespuesta.Mensaje = "Factura consultado con éxito.";
                    }

                    return Ok(FacturaRespuesta);
                }
            }
            catch (SqlException ex)
            {

                return StatusCode(500, new { message = "Error al consultar el Factura en la base de datos.", error = ex.Message });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { message = "Ocurrió un error inesperado al consultar el Factura.", error = ex.Message });
            }
        }

        /** [AllowAnonymous]
         [Route("ActualizarFactura")]
         [HttpPut]
         public IActionResult ActualizarFactura(FacturaEnt Factura)
         {
             var FacturaRespuesta = new FacturaRespuesta();
             try
             {
                 using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                 {
                     var result = db.Execute("ActualizarFactura",
                         new
                         {
                             Factura.IdProducto,
                             Factura.Cantidad,
                             Factura.IdCajero,
                             Factura.NuevaFactura,
                             Factura.Descuento
                         },
                         commandType: CommandType.StoredProcedure);

                     if (result <= 0)
                     {
                         FacturaRespuesta.Codigo = "-1";
                         FacturaRespuesta.Mensaje = "No se ha podido actualizar en la base de datos, intenta de nuevo";
                         return BadRequest(FacturaRespuesta);
                     }
                     else
                     {
                         FacturaRespuesta.Codigo = "1";
                         FacturaRespuesta.Mensaje = "Factura actualizado con éxito.";
                         return Ok(FacturaRespuesta);
                     }
                 }
             }
             catch (SqlException ex)
             {
                 return StatusCode(500, new { message = "Error al actualizar el Factura en la base de datos.", error = ex.Message });
             }
             catch (Exception ex)
             {
                 return StatusCode(500, new { message = "Ocurrió un error inesperado al actualizar el Factura.", error = ex.Message });
             }
         }
        **/

    }
}
