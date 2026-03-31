using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PuntoVentaAPI.Entities;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace PuntoVentaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarritoController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private static Carrito carrito = new Carrito();

        public CarritoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("GetCarrito")]
        public IActionResult GetCarrito()
        {
            return Ok(carrito);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("AgregarProducto/{productoId}")]
        public IActionResult AgregarProducto(string productoId, [FromBody] int cantidad)
        {
            using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var producto = db.Query<ProductoEnt>("ObtenerProductoPorId", new { IdProducto = productoId }, commandType: CommandType.StoredProcedure).FirstOrDefault();
                if (producto == null)
                {
                    return NotFound();
                }

                var carritoItem = carrito.Items.FirstOrDefault(i => i.ProductoId == productoId);
                if (carritoItem != null)
                {
                    carritoItem.Cantidad += cantidad;
                }
                else
                {
                    carrito.Items.Add(new CarritoItemEnt { ProductoId = productoId, Cantidad = cantidad, Producto = producto });
                }

                return Ok(carrito);
            }
        }


        [AllowAnonymous]
        [HttpDelete]
        [Route("EliminarProducto/{productoId}")]
        public IActionResult EliminarProducto(string productoId)
        {
            var carritoItem = carrito.Items.FirstOrDefault(i => i.ProductoId == productoId);
            if (carritoItem == null)
            {
                return NotFound();
            }

            carrito.Items.Remove(carritoItem);
            return Ok(carrito);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("VaciarCarrito")]
        public IActionResult VaciarCarrito()
        {
            carrito.Items.Clear();
            return Ok(carrito);
        }
    }
}
