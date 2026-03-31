using Dapper;
using Microsoft.AspNetCore.Mvc;
using PuntoVentaAPI.Entities;
using System.Data;
using System.Data.SqlClient;

namespace PuntoVentaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public DashboardController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("ObtenerDashboard")]
        public IActionResult ObtenerDashboard()
        {
            var dashboardRespuesta = new DashboardEnt.DashboardRespuesta();
            try
            {
                using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    using (var multi = db.QueryMultiple("ObtenerDatosDashboard", commandType: CommandType.StoredProcedure))
                    {
                        // Leer el primer conjunto de resultados (estadísticas del dashboard)
                        var stats = multi.Read().FirstOrDefault();
                        if (stats != null)
                        {
                            dashboardRespuesta.Dato = new DashboardEnt
                            {
                                TotalProveedores = (int)stats.TotalProveedores,
                                TotalProductos = (int)stats.TotalProductos,
                                TotalVentas = (int)stats.TotalVentas,
                                IngresosTotales = (decimal)stats.IngresosTotales,
                                TotalCategorias = (int)stats.TotalCategorias
                            };
                        }

                        // Leer el segundo conjunto de resultados (productos más vendidos)
                        var productosMasVendidos = multi.Read<string>().ToList();
                        dashboardRespuesta.Dato.ProductosMasVendidos = productosMasVendidos;

                        dashboardRespuesta.Codigo = "1";
                        dashboardRespuesta.Mensaje = "Datos del dashboard obtenidos con éxito.";
                        return Ok(dashboardRespuesta);
                    }
                }
            }
            catch (SqlException ex)
            {
                return StatusCode(500, new { message = "Error al obtener los datos del dashboard en la base de datos.", error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocurrió un error inesperado al obtener los datos del dashboard.", error = ex.Message });
            }
        }
    }
}
