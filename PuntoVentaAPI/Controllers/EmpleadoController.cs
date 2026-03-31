using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using PuntoVentaAPI.Entities;
using System.Data.SqlClient;
using Dapper;
using static PuntoVentaAPI.Entities.EmpleadoEnt;
using static PuntoVentaAPI.Entities.ProveedorEnt;

[Route("api/[controller]")]
[ApiController]
public class EmpleadoController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public EmpleadoController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpPost]
    [Route("RegistrarEmpleado")]
    public IActionResult RegistrarEmpleado(EmpleadoEnt empleado)
    {
        try
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var parameters = new DynamicParameters();
                parameters.Add("Cedula", empleado.Cedula, DbType.Int32, ParameterDirection.Input);
                parameters.Add("Nombre", empleado.Nombre, DbType.String, ParameterDirection.Input);
                parameters.Add("Apellido", empleado.Apellido, DbType.String, ParameterDirection.Input);
                parameters.Add("FechaIngreso", empleado.FechaIngreso, DbType.Date, ParameterDirection.Input);
                parameters.Add("HorasTrabajadas", empleado.HorasTrabajadas, DbType.Int32, ParameterDirection.Input);
                parameters.Add("HorasRebajadas", empleado.HorasRebajadas, DbType.Int32, ParameterDirection.Input);
                parameters.Add("ValorPorHora", empleado.ValorPorHora, DbType.Decimal, ParameterDirection.Input);

                connection.Execute("RegistrarEmpleado", parameters, commandType: CommandType.StoredProcedure);

                return Ok(new { mensaje = "Empleado registrado con éxito" });
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = "Error al registrar el empleado", error = ex.Message });
        }
    }




    [AllowAnonymous]
    [Route("ActualizarEmpleado")]
    [HttpPut]
    public IActionResult ActualizarEmpleado(EmpleadoEnt empleado)
    {
        EmpleadoRespuesta empleadoRespuesta = new EmpleadoRespuesta();
        try
        {
            using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var result = db.Execute("ActualizarEmpleado",
                    new
                    {
                        empleado.Cedula,
                        empleado.Nombre,
                        empleado.Apellido,
                        empleado.FechaIngreso,
                        empleado.HorasTrabajadas,
                        empleado.HorasRebajadas,
                        empleado.ValorPorHora
                    },
                    commandType: CommandType.StoredProcedure);

                if (result <= 0)
                {
                    empleadoRespuesta.Codigo = "-1";
                    empleadoRespuesta.Mensaje = "No se ha podido actualizar en la base de datos, intenta de nuevo";
                    return BadRequest(empleadoRespuesta);
                }
                else
                {
                    empleadoRespuesta.Codigo = "1";
                    empleadoRespuesta.Mensaje = "Empleado actualizado con éxito.";
                    return Ok(empleadoRespuesta);
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
    [Route("EliminarEmpleado")]
    [HttpDelete]
    public IActionResult EliminarEmpleado(int Cedula)
    {
        EmpleadoRespuesta empleadoRespuesta = new EmpleadoRespuesta();
        try
        {
            using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var result = db.Execute("EliminarEmpleado",
                    new
                    {
                        Cedula
                    },
                    commandType: CommandType.StoredProcedure);

                if (result <= 0)
                {
                    empleadoRespuesta.Codigo = "-1";
                    empleadoRespuesta.Mensaje = "No se ha podido eliminar el empleado en la base de datos, intenta de nuevo";
                    return BadRequest(empleadoRespuesta);
                }
                else
                {
                    empleadoRespuesta.Codigo = "1";
                    empleadoRespuesta.Mensaje = "Empleado eliminado con éxito.";
                    return Ok(empleadoRespuesta);
                }
            }
        }
        catch (SqlException ex)
        {

            return StatusCode(500, new { message = "Error al eliminar el empleado en la base de datos.", error = ex.Message });
        }
        catch (Exception ex)
        {

            return StatusCode(500, new { message = "Ocurrió un error inesperado al eliminar el empleado.", error = ex.Message });
        }
    }


    [AllowAnonymous]
    [Route("ConsultarEmpleados")]
    [HttpGet]
    public IActionResult ConsultarEmpleados()
    {
        EmpleadoRespuesta empleadoRespuesta = new EmpleadoRespuesta();
        try
        {
            using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var resultadoBD = db.Query<EmpleadoEnt>("ConsultarEmpleados", new { }, commandType: CommandType.StoredProcedure).ToList();

                if (resultadoBD == null || resultadoBD.Count == 0)
                {
                    empleadoRespuesta.Codigo = "-1";
                    empleadoRespuesta.Mensaje = "No hay empleados registrados.";
                }
                else
                {
                    empleadoRespuesta.Datos = resultadoBD;
                    empleadoRespuesta.Codigo = "1";
                    empleadoRespuesta.Mensaje = "Empleados consultados con éxito.";
                }
                return Ok(empleadoRespuesta);
            }
        }
        catch (SqlException ex)
        {

            return StatusCode(500, new { message = "Error al consultar empleados en la base de datos.", error = ex.Message });
        }
        catch (Exception ex)
        {

            return StatusCode(500, new { message = "Ocurrió un error inesperado al consultar empleados.", error = ex.Message });
        }
    }

    [AllowAnonymous]
    [Route("ObtenerEmpleadoPorCedula")]
    [HttpGet]
    public IActionResult ObtenerEmpleadoPorCedula(int Cedula)
    {
        EmpleadoRespuesta empleadoRespuesta = new EmpleadoRespuesta();
        try
        {
            using (var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var result = db.Query<EmpleadoEnt>("ObtenerEmpleadoPorCedula",
                    new { Cedula },
                    commandType: CommandType.StoredProcedure).FirstOrDefault();

                if (result == null)
                {
                    empleadoRespuesta.Codigo = "-1";
                    empleadoRespuesta.Mensaje = "No hay empleados registrados.";
                }
                else
                {
                    empleadoRespuesta.Dato = result;
                    empleadoRespuesta.Codigo = "1";
                    empleadoRespuesta.Mensaje = "Empleados consultado con éxito.";
                }

                return Ok(empleadoRespuesta);
            }
        }
        catch (SqlException ex)
        {

            return StatusCode(500, new { message = "Error al consultar el empleado en la base de datos.", error = ex.Message });
        }
        catch (Exception ex)
        {

            return StatusCode(500, new { message = "Ocurrió un error inesperado al consultar el empleado.", error = ex.Message });
        }
    }

}
