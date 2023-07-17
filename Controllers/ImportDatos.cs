using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Drawing.Text;
using static EnergiaDistribuida.ImportarExcel;


namespace EnergiaDistribuida.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImportDatos : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ImportarExcel _importarExcel;


        public ImportDatos(IConfiguration configuration, ImportarExcel importarExcel)
        {
            _configuration = configuration;
            _importarExcel = importarExcel;
        }

        [HttpGet("cargar-datos")]

        public IActionResult ImportarDatos()
        {
            try
            {
                _importarExcel.CargarDatosExcel();
                return Ok("Datos cargados desde el archivo excel exitosamente!.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al cargar los datos desde el archivo Excel: {ex.Message}");
            }
            
        }
    }
}
