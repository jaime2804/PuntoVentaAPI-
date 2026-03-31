using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PuntoVentaAPI.Entities;
using System.Data;
using System.Data.SqlClient;
using static PuntoVentaAPI.Entities.ProveedorEnt;

namespace PuntoVentaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProveedorController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ProveedorController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [AllowAnonymous]
        [Route("RegistrarProveedor")]
        [HttpPost]
        public IActionResult RegistrarProveedor(ProveedorEnt proveedor)
        {
            ProveedorRespuesta proveedorRespuesta = new ProveedorRespuesta();
            try
            {
                using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    // Verificar si el documento ya existe
                    var existe = db.QuerySingleOrDefault<int>(
                        "SELECT COUNT(1) FROM Proveedores WHERE NumeroDocumento = @NumeroDocumento",
                        new { proveedor.NumeroDocumento });

                    if (existe > 0)
                    {
                        proveedorRespuesta.Codigo = "-1";
                        proveedorRespuesta.Mensaje = "El número de documento ya está registrado.";
                        return BadRequest(proveedorRespuesta);
                    }

                    // Registrar el proveedor si no existe
                    var parametros = new
                    {
                        proveedor.NumeroDocumento,
                        proveedor.Nombre,
                        proveedor.Correo,
                        proveedor.Direccion,
                        proveedor.Telefono,
                        proveedor.Impuesto
                    };

                    var result = db.Execute("RegistrarProveedor", parametros, commandType: CommandType.StoredProcedure);
                    if (result > 0)
                    {
                        proveedorRespuesta.Codigo = "1";
                        proveedorRespuesta.Mensaje = "Proveedor registrado con éxito.";
                        return Ok(proveedorRespuesta);
                    }
                    else
                    {
                        proveedorRespuesta.Codigo = "-1";
                        proveedorRespuesta.Mensaje = "No se pudo registrar el proveedor.";
                        return BadRequest(proveedorRespuesta);
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocurrió un error al registrar el proveedor.", error = ex.Message });
            }
        }

        [AllowAnonymous]
        [Route("ConsultarProveedores")]
        [HttpGet]
        public IActionResult ConsultarProveedores()
        {
            ProveedorRespuesta proveedorRespuesta = new ProveedorRespuesta();
            try
            {
                using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var resultadoBD = db.Query<ProveedorEnt>("ConsultarProveedores", new { }, commandType: CommandType.StoredProcedure).ToList();

                    if (resultadoBD == null || resultadoBD.Count == 0)
                    {
                        proveedorRespuesta.Codigo = "-1";
                        proveedorRespuesta.Mensaje = "No hay proveedores registrados.";
                    }
                    else
                    {
                        proveedorRespuesta.Datos = resultadoBD;
                        proveedorRespuesta.Codigo = "1";
                        proveedorRespuesta.Mensaje = "Proveedores consultados con éxito.";
                    }
                    return Ok(proveedorRespuesta);
                }
            }
            catch (SqlException ex)
            {
               
                return StatusCode(500, new { message = "Error al consultar proveedores en la base de datos.", error = ex.Message });
            }
            catch (Exception ex)
            {
               
                return StatusCode(500, new { message = "Ocurrió un error inesperado al consultar proveedores.", error = ex.Message });
            }
        }


        [AllowAnonymous]
        [Route("ConsultarUnProveedor")]
        [HttpGet]
        public IActionResult ConsultarUnProveedor(long IdProveedor)
        {
            ProveedorRespuesta proveedorRespuesta = new ProveedorRespuesta();
            try
            {
                using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var result = db.Query<ProveedorEnt>("ObtenerProveedorPorID",
                        new { IdProveedor },
                        commandType: CommandType.StoredProcedure).FirstOrDefault();

                    if (result == null)
                    {
                        proveedorRespuesta.Codigo = "-1";
                        proveedorRespuesta.Mensaje = "No hay proveedores registrados.";
                    }
                    else
                    {
                        proveedorRespuesta.Dato = result;
                        proveedorRespuesta.Codigo = "1";
                        proveedorRespuesta.Mensaje = "Proveedor consultado con éxito.";
                    }

                    return Ok(proveedorRespuesta);
                }
            }
            catch (SqlException ex)
            {
                
                return StatusCode(500, new { message = "Error al consultar el proveedor en la base de datos.", error = ex.Message });
            }
            catch (Exception ex)
            {
               
                return StatusCode(500, new { message = "Ocurrió un error inesperado al consultar el proveedor.", error = ex.Message });
            }
        }



        [AllowAnonymous]
        [Route("ActualizarProveedor")]
        [HttpPut]
        public IActionResult ActualizarProveedor(ProveedorEnt proveedor)
        {
            ProveedorRespuesta proveedorRespuesta = new ProveedorRespuesta();
            try
            {
                using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var result = db.Execute("ActualizarProveedor",
                        new
                        {
                            proveedor.IdProveedor,
                  
                            proveedor.NumeroDocumento,
                            proveedor.Nombre,
                            proveedor.Correo,
                            proveedor.Direccion,
                            proveedor.Telefono,
                            proveedor.Impuesto
                        },
                        commandType: CommandType.StoredProcedure);

                    if (result <= 0)
                    {
                        proveedorRespuesta.Codigo = "-1";
                        proveedorRespuesta.Mensaje = "No se ha podido actualizar en la base de datos, intenta de nuevo";
                        return BadRequest(proveedorRespuesta);
                    }
                    else
                    {
                        proveedorRespuesta.Codigo = "1";
                        proveedorRespuesta.Mensaje = "Proveedor actualizado con éxito.";
                        return Ok(proveedorRespuesta);
                    }
                }
            }
            catch (SqlException ex)
            {
                
                return StatusCode(500, new { message = "Error al actualizar el proveedor en la base de datos.", error = ex.Message });
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, new { message = "Ocurrió un error inesperado al actualizar el proveedor.", error = ex.Message });
            }
        }

        [AllowAnonymous]
        [Route("EliminarProveedor")]
        [HttpDelete]
        public IActionResult EliminarProveedor(long IdProveedor)
        {
            ProveedorRespuesta proveedorRespuesta = new ProveedorRespuesta();
            try
            {
                using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var result = db.Execute("EliminarProveedor",
                        new
                        {
                            IdProveedor
                        },
                        commandType: CommandType.StoredProcedure);

                    if (result <= 0)
                    {
                        proveedorRespuesta.Codigo = "-1";
                        proveedorRespuesta.Mensaje = "No se ha podido eliminar el proveedor en la base de datos, intenta de nuevo";
                        return BadRequest(proveedorRespuesta);
                    }
                    else
                    {
                        proveedorRespuesta.Codigo = "1";
                        proveedorRespuesta.Mensaje = "Proveedor eliminado con éxito.";
                        return Ok(proveedorRespuesta);
                    }
                }
            }
            catch (SqlException ex)
            {
         
                return StatusCode(500, new { message = "Error al eliminar el proveedor en la base de datos.", error = ex.Message });
            }
            catch (Exception ex)
            {
              
                return StatusCode(500, new { message = "Ocurrió un error inesperado al eliminar el proveedor.", error = ex.Message });
            }
        }

    }
}


