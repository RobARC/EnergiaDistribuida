using System;
using System.Collections.Generic;

namespace EnergiaDistribuida.Models;

public partial class Cliente
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string ActividadEconomica { get; set; } = null!;

    public long? Consumo { get; set; }

    public long? Costo { get; set; }

    public long? GananciaOperdida { get; set; }

    public int TipoClienteId { get; set; }

    public int TramoId { get; set; }

    public virtual TiposCliente TipoCliente { get; set; } = null!;

    public virtual Tramo Tramo { get; set; } = null!;
}
