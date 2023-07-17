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
    public class ConsumosPorClientesController : ControllerBase
    {
        private readonly EnergiaDistribuidaContext _context;
        private readonly IConfiguration _configuration;

        public ConsumosPorClientesController(EnergiaDistribuidaContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/CostosPorTramos
        [HttpGet]
        public IActionResult ObtenerConsumosCostosPorClientes(DateTime fechaInicial, DateTime fechaFinal)
        {
            //Conectamos con la BD
            string connectionString = _configuration.GetConnectionString("MSSQLServerConnection");
            using var connection = new SqlConnection(connectionString);
            connection.Open();

            using var command = new SqlCommand("ObtenerConsumoCostoPorCliente", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@FechaInicial", fechaInicial);
            command.Parameters.AddWithValue("@FechaFinal", fechaFinal);

            var result = new List<HistoricoConsumoPorClientes>();

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var tramo = reader.GetString(0);
                var consumoResidencial = reader.GetDecimal(2);
                var consumoComercial = reader.GetDecimal(2);
                var consumoIndustrial = reader.GetDecimal(2);
                var costoResidencial = reader.GetDecimal(2);
                var costoComercial = reader.GetDecimal(2);
                var costoIndustrial = reader.GetDecimal(2);

                var registro = new HistoricoConsumoPorClientes
                {
                    Tramo = tramo,
                    TotalConsumoResidencial = consumoResidencial,
                    TotalConsumoComercial = consumoComercial,
                    TotalConsumoIndustrial = consumoIndustrial,
                    TotalCostoResidencial = costoResidencial,
                    TotalCostoComercial = costoComercial,
                    TotalCostoIndustrial = costoIndustrial
                };

                result.Add(registro);
            }

            return Ok(result);
        }

       
    }
}
