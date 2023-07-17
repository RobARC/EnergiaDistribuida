namespace EnergiaDistribuida.Dtos
{
    public class HistoricoConsumoTramo
    {
        public string? Tramo { get; set; }
        public DateTime? Date { get; set; }
        public decimal Consumo { get; set; }
        public decimal? Perdidas { get; set; }
        public decimal Costo { get; set; }
    }
}
