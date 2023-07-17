using System;
using System.Collections.Generic;

namespace EnergiaDistribuida.Models;

public partial class CostosPorTramo
{
    public int Id { get; set; }

    public string? Tramo { get; set; }

    public DateTime Fecha { get; set; }

    public long CostosResidencial { get; set; }

    public long CostosComercial { get; set; }

    public long CostosIndustrial { get; set; }
}
