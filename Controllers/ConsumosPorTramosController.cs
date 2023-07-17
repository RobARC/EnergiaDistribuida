using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EnergiaDistribuida.Models;
using Microsoft.Data.SqlClient;
using System.Configuration;
using EnergiaDistribuida.Dtos;
using System.Data;

namespace EnergiaDistribuida.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsumosPorTramosController : ControllerBase
    {
        private readonly EnergiaDistribuidaContext _context;
        private readonly IConfiguration _configuration;

        public ConsumosPorTramosController(EnergiaDistribuidaContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/ConsumoPorTramoes
        [HttpGet]
        public IActionResult ObtenerConsumosCostosPorTramos(DateTime fechaInicial, DateTime fechaFinal)
        {
            //Conectamos con la BD
            string connectionString = _configuration.GetConnectionString("MSSQLServerConnection");
            using var connection = new SqlConnection(connectionString);
            connection.Open();

            using var command = new SqlCommand("ObtenerConsumoCostoPorTramo", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@FechaInicial", fechaInicial);
            command.Parameters.AddWithValue("@FechaFinal", fechaFinal);

            var result = new List<HistoricoConsumoTramo>();

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var fecha = reader.GetDateTime(0);
                var tramo = reader.GetString(1);
                var consumo = reader.GetDecimal(2);
                var costo = reader.GetDecimal(3);

                var registro = new HistoricoConsumoTramo
                {
                    Tramo = tramo,
                    Date = fecha,
                    Consumo = consumo,
                    Costo = costo
                };

                result.Add(registro);
            }

            return Ok(result);

        }
    }
}
