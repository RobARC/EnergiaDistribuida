using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EnergiaDistribuida.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using EnergiaDistribuida.Dtos;

namespace EnergiaDistribuida.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PerdidasPorTramosController : ControllerBase
    {
        private readonly EnergiaDistribuidaContext _context;
        private readonly IConfiguration _configuration;

        public PerdidasPorTramosController(EnergiaDistribuidaContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/PerdidasPorTramoes
        [HttpGet]
        public IActionResult ObtenerTop20PerdidasClientes(DateTime fechaInicial, DateTime fechaFinal)
        {
            //Conectamos con la BD
            string connectionString = _configuration.GetConnectionString("MSSQLServerConnection");
            using var connection = new SqlConnection(connectionString);
            connection.Open();

            using var command = new SqlCommand("Top20TramosClientes", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@FechaInicial", fechaInicial);
            command.Parameters.AddWithValue("@FechaFinal", fechaFinal);

            var result = new List<Top20PerdidasTramosClientes>();

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var tramo = reader.GetString(0);
                var resindecial = reader.GetDecimal(1);
                var comercial = reader.GetDecimal(2);
                var industrial = reader.GetDecimal(3);

                var registro = new Top20PerdidasTramosClientes
                {
                    Tramo = tramo,
                    Residencial = resindecial,
                    Comercial = comercial,
                    Industrial = industrial
                };

                result.Add(registro);
            }

            return Ok(result);
        }
        
    }
}
