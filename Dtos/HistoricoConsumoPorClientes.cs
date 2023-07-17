namespace EnergiaDistribuida.Dtos
{
    public class HistoricoConsumoPorClientes
    {
        public string? Tramo { get; set; }
        public decimal TotalConsumoResidencial { get; set; }
        public decimal TotalConsumoComercial { get; set; }
        public decimal TotalConsumoIndustrial { get; set; }
        public decimal TotalCostoResidencial { get; set; }
        public decimal TotalCostoComercial { get; set; }
        public decimal TotalCostoIndustrial { get; set; }
    }
}
