using OfficeOpenXml;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Globalization;
using SpreadsheetLight;
using EnergiaDistribuida.Models;


namespace EnergiaDistribuida
{
    public class ImportarExcel
    {
        private readonly IConfiguration _configuration;

        public ImportarExcel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void CargarDatosExcel()
        {
            string nombreArchivo = "EPSA_Listado_Costos.xlsx";
            string carpetaDescargas = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
            string rutaArchivoExcel = Path.Combine(carpetaDescargas, nombreArchivo);
            string connectionString = _configuration.GetConnectionString("MSSQLServerConnection");
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using var package = new ExcelPackage(new System.IO.FileInfo(rutaArchivoExcel));
            SLDocument document = new SLDocument(rutaArchivoExcel);

            using var connection = new SqlConnection(connectionString);
            connection.Open();

            CargarHojaConsumoPorTramo(document, connection);
            CargarHojaCostosPorCliente(package, connection);
            CargarHojaPerdidasPorTramo(package, connection);

            connection.Close();
            
        }
        private void CargarHojaConsumoPorTramo(SLDocument document, SqlConnection connection)
        {
            //var worksheet = package.Workbook.Worksheets["CONSUMO_POR_TRAMO"];
            document.SelectWorksheet("CONSUMO_POR_TRAMO");

            var dataTable = new DataTable();
            dataTable.Columns.Add("Id", typeof(int));
            dataTable.Columns.Add("Linea", typeof(string));
            dataTable.Columns.Add("Fecha", typeof(DateTime));
            dataTable.Columns.Add("Residencial [Wh]", typeof(int));
            dataTable.Columns.Add("Comercial [Wh]", typeof(int));
            dataTable.Columns.Add("Industrial [Wh]", typeof(int));

            SLWorksheetStatistics stats = document.GetWorksheetStatistics();

            int totalFilas = stats.EndRowIndex;

            for (int row = 2; row <= totalFilas; row++) // Rango de filas en la hoja CONSUMO_POR_TRAMO
            {
                int Id = 0;
                var tramo = document.GetCellValueAsString(row, 1);

                //Formatear Fecha
                string fechaNumeroString = document.GetCellValueAsString(row, 2);
                double fechaNumero = double.Parse(fechaNumeroString);
                DateTime fecha = DateTime.FromOADate(fechaNumero);
                string fechaFormateada = fecha.ToString("yyyy-MM-dd");

                //var fechaFormateada = DateTime.Parse(worksheet.Cells[row, 2].Value?.ToString());
                var residencial = document.GetCellValueAsInt32(row, 3);
                var comercial = document.GetCellValueAsInt32(row, 4); ;
                var industrial = document.GetCellValueAsInt32(row, 5);

                dataTable.Rows.Add(Id, tramo, fechaFormateada, residencial, comercial, industrial);
            }

            BulkInsertData("ConsumoPorTramo", dataTable, connection);
        }

        private void CargarHojaCostosPorCliente(ExcelPackage package, SqlConnection connection)
        {
            var worksheet = package.Workbook.Worksheets["COSTOS_POR_TRAMO"];
            var dataTable = new DataTable();
            dataTable.Columns.Add("Id", typeof(int));
            dataTable.Columns.Add("Linea", typeof(string));
            dataTable.Columns.Add("Fecha", typeof(DateTime));
            dataTable.Columns.Add("Residencial [Costo/Wh]", typeof(int));
            dataTable.Columns.Add("Comercial [Costo/Wh]", typeof(int));
            dataTable.Columns.Add("Industrial [Costo/Wh]", typeof(int));
        
            int columnaIndex = 3; // Formateamos la Columna 3
        
            var columna = worksheet.Cells[2, columnaIndex, worksheet.Dimension.End.Row, columnaIndex];
            columna.Style.Numberformat.Format = "0";
        
            int totalFilas = worksheet.Dimension.Rows;
        
        
            for (int row = 2; row <= totalFilas; row++) // Rango de filas en la hoja COSTOS_POR_TRAMO
            {
                var Id = 0;
                var tramo = worksheet.Cells[row, 1].Value?.ToString();
        
                //Formatear Fecha
                string fechaNumeroString = worksheet.Cells[row, 2].Value?.ToString();
                double fechaNumero = double.Parse(fechaNumeroString);
                DateTime fecha = DateTime.FromOADate(fechaNumero);
                string fechaFormateada = fecha.ToString("yyyy-MM-dd");
        
                var residencial = Decimal.Parse(worksheet.Cells[row, 3].Value?.ToString());
                var comercial = Decimal.Parse(worksheet.Cells[row, 4].Value?.ToString());
                var industrial = Decimal.Parse(worksheet.Cells[row, 5].Value?.ToString());
        
                dataTable.Rows.Add(Id, tramo, fechaFormateada, residencial, comercial, industrial);
            }
        
            BulkInsertData("CostosPorTramo", dataTable, connection);
        }
        
        private void CargarHojaPerdidasPorTramo(ExcelPackage package, SqlConnection connection)
        {
            var worksheet = package.Workbook.Worksheets["PERDIDAS_POR_TRAMO"];
            var dataTable = new DataTable();
            dataTable.Columns.Add("Id", typeof(int));
            dataTable.Columns.Add("Linea", typeof(string));
            dataTable.Columns.Add("Fecha", typeof(DateTime));
            dataTable.Columns.Add("Residencial [%]", typeof(int));
            dataTable.Columns.Add("Comercial [%]", typeof(int));
            dataTable.Columns.Add("Industrial [%]", typeof(int));
        
            int totalFilas = worksheet.Dimension.Rows;
        
            for (int row = 2; row <= totalFilas; row++) // Rango de filas en la hoja PERDIDAS_POR_TRAMO
            {
                var Id = 0;
                var tramo = worksheet.Cells[row, 1].Value?.ToString();
        
                //Formatear Fecha
                string fechaNumeroString = worksheet.Cells[row, 2].Value?.ToString();
                double fechaNumero = double.Parse(fechaNumeroString);
                DateTime fecha = DateTime.FromOADate(fechaNumero);
                string fechaFormateada = fecha.ToString("yyyy-MM-dd");
        
                // Formatear valores a porcentajes
                string residencialString = worksheet.Cells[row, 3].Value?.ToString();
                decimal residencial;
        
                if (!decimal.TryParse(residencialString, out residencial))
                {
                    if (double.TryParse(residencialString, out double residencialDouble))
                    {
                        residencial = Convert.ToDecimal(residencialDouble);
                    }
                    else
                    {
                        Console.WriteLine("No se pudo convertir a Decimal, favor verificar datos de entrada");
                    }
                }
                residencial *= 100;
        
                string comercialString = worksheet.Cells[row, 4].Value?.ToString();
                decimal comercial;
        
                if (!decimal.TryParse(comercialString, out comercial))
                {
                    if (double.TryParse(comercialString, out double comercialDouble))
                    {
                        comercial = Convert.ToDecimal(comercialDouble);
                    }
                    else
                    {
                        Console.WriteLine("No se pudo convertir a Decimal, favor verificar datos de entrada");
                    }
                }
                comercial*= 100;
        
                string industrialString = worksheet.Cells[row, 5].Value?.ToString();
                decimal industrial;
        
                if (!decimal.TryParse(industrialString, out industrial))
                {
                    if (double.TryParse(industrialString, out double industrialDouble))
                    {
                        industrial = Convert.ToDecimal(industrialDouble);
                    }
                    else
                    {
                        Console.WriteLine("No se pudo convertir a Decimal, favor verificar datos de entrada");
                    }
                }
                industrial*= 100;
        
                dataTable.Rows.Add(Id, tramo, fechaFormateada, residencial, comercial, industrial);
            }
        
            BulkInsertData("PerdidasPorTramo", dataTable, connection);
        }
        private void BulkInsertData(string tableName, DataTable dataTable, SqlConnection connection)
        {
            using (var bulkCopy = new SqlBulkCopy(connection))
            {
                bulkCopy.DestinationTableName = tableName;
                bulkCopy.WriteToServer(dataTable);
            }
        }
}
}

    

   




    

