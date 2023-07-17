using System;
using System.Collections.Generic;

namespace EnergiaDistribuida.Models;

public partial class ConsumoPorTramo
{
    public int Id { get; set; }

    public string? Tramo { get; set; }

    public DateTime Fecha { get; set; }

    public decimal ConsumoResidencial { get; set; }

    public decimal ConsumoComercial { get; set; }

    public decimal ConsumoIndustrial { get; set; }
}
