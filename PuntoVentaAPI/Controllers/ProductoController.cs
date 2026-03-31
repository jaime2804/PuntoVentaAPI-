using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PuntoVentaAPI.Entities;
using System.Data;
using System.Data.SqlClient;

namespace PuntoVentaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ProductoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [AllowAnonymous]
        [Route("RegistrarProducto")]
        [HttpPost]
        public IActionResult RegistrarProducto(ProductoEnt producto)
        {
            var productoRespuesta = new ProductoRespuesta();
            try
            {
                using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    // Verifica si la categoría existe
                    var categoria = db.Query<CategoriaEnt>("SELECT * FROM Categoria WHERE Id = @Id", new { Id = producto.IdCategoria }).FirstOrDefault();
                    if (categoria == null)
                    {
                        productoRespuesta.Codigo = "-1";
                        productoRespuesta.Mensaje = "La categoría especificada no existe.";
                      //  return BadRequest(productoRespuesta);
                    }

                    var parametros = new
                    {
                        producto.IdProducto,
                        producto.Nombre,
                        producto.Precio,
                        producto.Stock,
                        producto.IdCategoria
                    };

                    var result = db.Execute("RegistrarProducto", parametros, commandType: CommandType.StoredProcedure);
                    if (result > 0)
                    {
                        productoRespuesta.Codigo = "1";
                        productoRespuesta.Mensaje = "Producto registrado con éxito.";
                        return Ok(productoRespuesta);
                    }
                    else
                    {
                        productoRespuesta.Codigo = "-1";
                        productoRespuesta.Mensaje = "No se pudo registrar el producto.";
                        return BadRequest(productoRespuesta);
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocurrió un error al registrar el producto.", error = ex.Message });
            }
        }


        [AllowAnonymous]
        [Route("ConsultarProductos")]
        [HttpGet]
        public IActionResult ConsultarProductos()
        {
            var productoRespuesta = new ProductoRespuesta();
            try
            {
                using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var resultadoBD = db.Query<ProductoEnt>("ConsultarProductos", commandType: CommandType.StoredProcedure).ToList();

                    if (resultadoBD == null || resultadoBD.Count == 0)
                    {
                        productoRespuesta.Codigo = "-1";
                        productoRespuesta.Mensaje = "No hay productos registrados.";
                    }
                    else
                    {
                        productoRespuesta.Datos = resultadoBD;
                        productoRespuesta.Codigo = "1";
                        productoRespuesta.Mensaje = "Productos consultados con éxito.";
                    }
                    return Ok(productoRespuesta);
                }
            }
            catch (SqlException ex)
            {
                return StatusCode(500, new { message = "Error al consultar productos en la base de datos.", error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocurrió un error inesperado al consultar productos.", error = ex.Message });
            }
        }

        [AllowAnonymous]
        [Route("ConsultarUnProducto")]
        [HttpGet]
        public IActionResult ConsultarUnProducto(string idProducto)
        {
            var productoRespuesta = new ProductoRespuesta();
            try
            {
                using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var result = db.Query<ProductoEnt>("ObtenerProductoPorID",
                        new { IdProducto = idProducto },
                        commandType: CommandType.StoredProcedure).FirstOrDefault();

                    if (result == null)
                    {
                        productoRespuesta.Codigo = "-1";
                        productoRespuesta.Mensaje = "No hay productos registrados.";
                    }
                    else
                    {
                        productoRespuesta.Dato = result;
                        productoRespuesta.Codigo = "1";
                        productoRespuesta.Mensaje = "Producto consultado con éxito.";
                    }

                    return Ok(productoRespuesta);
                }
            }
            catch (SqlException ex)
            {
                return StatusCode(500, new { message = "Error al consultar el producto en la base de datos.", error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocurrió un error inesperado al consultar el producto.", error = ex.Message });
            }
        }

        [AllowAnonymous]
        [Route("ActualizarProducto")]
        [HttpPut]
        public IActionResult ActualizarProducto(ProductoEnt producto)
        {
            var productoRespuesta = new ProductoRespuesta();
            try
            {
                using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var result = db.Execute("ActualizarProducto",
                        new
                        {
                            producto.IdProducto,
                            producto.Nombre,
                            producto.Precio,
                            producto.Stock,
                            producto.IdCategoria
                        },
                        commandType: CommandType.StoredProcedure);

                    if (result <= 0)
                    {
                        productoRespuesta.Codigo = "-1";
                        productoRespuesta.Mensaje = "No se ha podido actualizar en la base de datos, intenta de nuevo";
                        return BadRequest(productoRespuesta);
                    }
                    else
                    {
                        productoRespuesta.Codigo = "1";
                        productoRespuesta.Mensaje = "Producto actualizado con éxito.";
                        return Ok(productoRespuesta);
                    }
                }
            }
            catch (SqlException ex)
            {
                return StatusCode(500, new { message = "Error al actualizar el producto en la base de datos.", error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocurrió un error inesperado al actualizar el producto.", error = ex.Message });
            }
        }

        [AllowAnonymous]
        [Route("EliminarProducto")]
        [HttpDelete]
        public IActionResult EliminarProducto(string idProducto)
        {
            var productoRespuesta = new ProductoRespuesta();
            try
            {
                using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var result = db.Execute("EliminarProducto",
                        new { IdProducto = idProducto },
                        commandType: CommandType.StoredProcedure);

                    if (result <= 0)
                    {
                        productoRespuesta.Codigo = "-1";
                        productoRespuesta.Mensaje = "No se ha podido eliminar el producto en la base de datos, intenta de nuevo";
                        return BadRequest(productoRespuesta);
                    }
                    else
                    {
                        productoRespuesta.Codigo = "1";
                        productoRespuesta.Mensaje = "Producto eliminado con éxito.";
                        return Ok(productoRespuesta);
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
