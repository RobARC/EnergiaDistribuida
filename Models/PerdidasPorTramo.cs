using System;
using System.Collections.Generic;

namespace EnergiaDistribuida.Models;

public partial class PerdidasPorTramo
{
    public int Id { get; set; }

    public string? Tramo { get; set; }

    public DateTime Fecha { get; set; }

    public decimal PerdidaResidencial { get; set; }

    public decimal PerdidaComercial { get; set; }

    public decimal PerdidaIndustrial { get; set; }
}
