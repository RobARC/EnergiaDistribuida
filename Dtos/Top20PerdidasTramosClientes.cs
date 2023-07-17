namespace EnergiaDistribuida.Dtos
{
    public class Top20PerdidasTramosClientes
    {
        public string? Tramo { get; set; }
        public decimal Residencial { get; set; }
        public decimal? Comercial { get; set; }
        public decimal Industrial { get; set; }
    }
}
