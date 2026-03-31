using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using PuntoVentaAPI.Entities;
using System.Data.SqlClient;
using Dapper;
using static PuntoVentaAPI.Entities.ArqueoEnt;
using static PuntoVentaAPI.Entities.MovimientoEnt;
using static PuntoVentaAPI.Entities.CajaEnt;

namespace PuntoVentaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArqueoController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ArqueoController(IConfiguration configuration)
        {
            _configuration = configuration;

        }

        //------------------------------------------------------------------------------------------Caja---------------------------------------------------------------------------------------- //

        [AllowAnonymous]
        [Route("CrearCaja")]
        [HttpPost]
        public IActionResult CrearCaja(CajaEnt entidad)
        {
            CajaRespuesta CajaRespuesta = new CajaRespuesta();
            try
            {
                using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var parametros = new
                    {
                        entidad.MontoInicial
                    };

                    var result = db.Execute("CrearCaja", parametros, commandType: CommandType.StoredProcedure);
                    if (result > 0)
                    {
                        CajaRespuesta.Codigo = "1";
                        CajaRespuesta.Mensaje = "Caja registrada con éxito.";
                        return Ok(CajaRespuesta);
                    }
                    else
                    {
                        CajaRespuesta.Codigo = "-1";
                        CajaRespuesta.Mensaje = "No se pudo registrar la caja.";
                        return BadRequest(CajaRespuesta);
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocurrió un error al registrar la caja.", error = ex.Message });
            }
        }


        [AllowAnonymous]
        [Route("ObtenerCajaPorId")]
        [HttpGet]
        public IActionResult ObtenerCajaPorId(int IdCaja)
        {
            CajaRespuesta CajaRespuesta = new CajaRespuesta();
            try
            {
                using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var result = db.Query<CajaEnt>("ObtenerCajaPorId",
                        new { IdCaja },
                        commandType: CommandType.StoredProcedure).FirstOrDefault(); 

                    if (result == null)
                    {
                        CajaRespuesta.Codigo = "-1";
                        CajaRespuesta.Mensaje = "No hay caja registradas.";
                    }
                    else
                    {
                        CajaRespuesta.Dato = result;
                        CajaRespuesta.Codigo = "1";
                        CajaRespuesta.Mensaje = "Caja consultada con éxito.";
                    }

                    return Ok(CajaRespuesta);
                }
            }
            catch (SqlException ex)
            {

                return StatusCode(500, new { message = "Error al consultar la caja en la base de datos.", error = ex.Message });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { message = "Ocurrió un error inesperado al consultar la caja.", error = ex.Message });
            }
        }


        [AllowAnonymous]
        [Route("CalcularCierreSemanal")]
        [HttpGet]
        public IActionResult CalcularCierreSemanal()
        {
            var respuesta = new CierreSemanalRespuesta();
            try
            {
                using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var resultadoBD = db.Query<CierreSemanalEnt>("CalcularCierreSemanal", commandType: CommandType.StoredProcedure).ToList();

                    if (resultadoBD == null || resultadoBD.Count == 0)
                    {
                        respuesta.Codigo = "-1";
                        respuesta.Mensaje = "No hay cierres de cajas registrados.";
                    }
                    else
                    {
                        respuesta.Datos = resultadoBD;
                        respuesta.Codigo = "1";
                        respuesta.Mensaje = "Cierre de caja semanal consultado con éxito.";
                    }
                    return Ok(respuesta);
                }
            }
            catch (SqlException ex)
            {
                return StatusCode(500, new { message = "Error al consultar el arqueo en la base de datos.", error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocurrió un error inesperado al consultar el arqueo.", error = ex.Message });
            }
        }


        //------------------------------------------------------------------------------------------Arqueos---------------------------------------------------------------------------------------- //

        [AllowAnonymous]
        [Route("ObtenerTodosArqueos")]
        [HttpGet]
        public IActionResult ObtenerTodosArqueos()
        {
            var respuesta = new CajaRespuesta();
            try
            {
                using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var resultadoBD = db.Query<CajaEnt>("ObtenerTodosArqueos", commandType: CommandType.StoredProcedure).ToList();

                    if (resultadoBD == null || resultadoBD.Count == 0)
                    {
                        respuesta.Codigo = "-1";
                        respuesta.Mensaje = "No hay arqueos registrados.";
                    }
                    else
                    {
                        respuesta.Datos = resultadoBD;
                        respuesta.Codigo = "1";
                        respuesta.Mensaje = "Arqueo consultados con éxito.";
                    }
                    return Ok(respuesta);
                }
            }
            catch (SqlException ex)
            {
                return StatusCode(500, new { message = "Error al consultar el arqueo en la base de datos.", error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocurrió un error inesperado al consultar el arqueo.", error = ex.Message });
            }
        }

       



        [AllowAnonymous]
        [Route("RegistrarArqueo")]
        [HttpPost]
        public IActionResult RegistrarArqueo(ArqueoEnt arqueo)
        {
            var respuesta = new ArqueoRespuesta();
            try
            {
                using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var parametros = new
                    {
                        arqueo.IdCaja,
                        arqueo.Billetes1000,
                        arqueo.Billetes2000,
                        arqueo.Billetes5000,
                        arqueo.Billetes10000,
                        arqueo.Billetes20000,
                        arqueo.Monedas5,
                        arqueo.Monedas10,
                        arqueo.Monedas25,
                        arqueo.Monedas50,
                        arqueo.Monedas100,
                        arqueo.Monedas500
                    };

                    var result = db.Execute("RegistrarArqueo", parametros, commandType: CommandType.StoredProcedure);
                    if (result > 0)
                    {
                        respuesta.Codigo = "1";
                        respuesta.Mensaje = "Arqueo registrado con éxito.";
                        respuesta.Dato = arqueo; // Aquí se puede devolver el arqueo registrado si es necesario.
                        return Ok(respuesta);
                    }
                    else
                    {
                        respuesta.Codigo = "-1";
                        respuesta.Mensaje = "No se pudo registrar el arqueo.";
                        return BadRequest(respuesta);
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocurrió un error al registrar el arqueo.", error = ex.Message });
            }
        }

        [AllowAnonymous]
        [Route("CalcularTotalesArqueo")]
        [HttpGet]
        public IActionResult CalcularTotalesArqueo(int idArqueo)
        {
            var arqueoRespuesta = new ArqueoTotalesRespuesta();
            try
            {
                using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var result = db.Query<ArqueoTotalesEnt>("CalcularTotalesArqueo",
                        new { IdArqueo = idArqueo },
                        commandType: CommandType.StoredProcedure).FirstOrDefault();

                    if (result == null)
                    {
                        arqueoRespuesta.Codigo = "-1";
                        arqueoRespuesta.Mensaje = "No hay arqueos registrados.";
                    }
                    else
                    {
                        arqueoRespuesta.Dato = result;
                        arqueoRespuesta.Codigo = "1";
                        arqueoRespuesta.Mensaje = "Arqueo consultado con éxito.";
                    }

                    return Ok(arqueoRespuesta);
                }
            }
            catch (SqlException ex)
            {
                return StatusCode(500, new { message = "Error al consultar el movimiento en la base de datos.", error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocurrió un error inesperado al consultar el movimiento.", error = ex.Message });
            }
        }



        //------------------------------------------------------------------------------------------Movimientos---------------------------------------------------------------------------------------- //

        [AllowAnonymous]
        [Route("ObtenerMovimientos")]
        [HttpGet]
        public IActionResult ObtenerMovimientos()
        {
            var respuesta = new MovimientoRespuesta();
            try
            {
                using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var resultadoBD = db.Query<MovimientoEnt>("ObtenerMovimientos", commandType: CommandType.StoredProcedure).ToList();

                    if (resultadoBD == null || resultadoBD.Count == 0)
                    {
                        respuesta.Codigo = "-1";
                        respuesta.Mensaje = "No hay movimiento registrado.";
                    }
                    else
                    {
                        respuesta.Movimientos = resultadoBD;
                        respuesta.Codigo = "1";
                        respuesta.Mensaje = "Movimiento consultado con éxito.";
                    }
                    return Ok(respuesta);
                }
            }
            catch (SqlException ex)
            {
                return StatusCode(500, new { message = "Error al consultar el movimiento en la base de datos.", error = ex.Message });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocurrió un error inesperado al consultar el movimiento.", error = ex.Message });
            }
        }


        [AllowAnonymous]
        [Route("ConsultarUnMovimiento")]
        [HttpGet]
        public IActionResult ConsultarUnMovimiento(int idCaja)
        {
            var movimientoRespuesta = new MovimientoRespuesta();
            try
            {
                using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var result = db.Query<MovimientoEnt>("ObtenerMovimientoPorId",
                        new { IdCaja = idCaja },
                        commandType: CommandType.StoredProcedure).ToList();

                    if (result == null)
                    {
                        movimientoRespuesta.Codigo = "-1";
                        movimientoRespuesta.Mensaje = "No hay movimientos registrados.";
                    }
                    else
                    {
                        movimientoRespuesta.Datos = result;
                        movimientoRespuesta.Codigo = "1";
                        movimientoRespuesta.Mensaje = "Movimiento consultado con éxito.";
                    }

                    return Ok(movimientoRespuesta);
                }
            }
            catch (SqlException ex)
            {
                return StatusCode(500, new { message = "Error al consultar el movimiento en la base de datos.", error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocurrió un error inesperado al consultar el movimiento.", error = ex.Message });
            }
        }

        [AllowAnonymous]
        [Route("RegistrarMovimiento")]
        [HttpPost]
        public IActionResult RegistrarMovimiento(MovimientoEnt movimiento)
        {
            var respuesta = new ArqueoRespuesta();
            try
            {
                using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var parametros = new
                    {
                        movimiento.IdCaja,
                        movimiento.TipoMovimiento,
                        movimiento.Monto,
                        movimiento.Descripcion
                    };

                    var result = db.Execute("RegistrarMovimiento", parametros, commandType: CommandType.StoredProcedure);
                    if (result > 0)
                    {
                        respuesta.Codigo = "1";
                        respuesta.Mensaje = "Movimiento registrado con éxito.";
                        return Ok(respuesta);
                    }
                    else
                    {
                        respuesta.Codigo = "-1";
                        respuesta.Mensaje = "No se pudo registrar el movimiento.";
                        return BadRequest(respuesta);
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocurrió un error al registrar el movimiento.", error = ex.Message });
            }
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- //

        [AllowAnonymous]
        [Route("EliminarCaja")]
        [HttpDelete]
        public IActionResult EliminarCaja(int idCaja)
        {
            var cajaRespuesta = new CajaRespuesta();
            try
            {
                using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var result = db.Execute("EliminarCaja",
                        new { IdCaja = idCaja },
                        commandType: CommandType.StoredProcedure);

                    if (result <= 0)
                    {
                        cajaRespuesta.Codigo = "-1";
                        cajaRespuesta.Mensaje = "No se ha podido eliminar la caja en la base de datos, intenta de nuevo";
                        return BadRequest(cajaRespuesta);
                    }
                    else
                    {
                        cajaRespuesta.Codigo = "1";
                        cajaRespuesta.Mensaje = "Caja eliminada con éxito.";
                        return Ok(cajaRespuesta);
                    }
                }
            }
            catch (SqlException ex)
            {
                return StatusCode(500, new { message = "Error al eliminar el producto en la base de datos.", error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocurrió un error inesperado al eliminar el producto.", error = ex.Message });
            }
        }
    }
}