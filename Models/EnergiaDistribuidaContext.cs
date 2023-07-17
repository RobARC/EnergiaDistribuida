using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EnergiaDistribuida.Models;

public partial class EnergiaDistribuidaContext : DbContext
{
    public EnergiaDistribuidaContext()
    {
    }

    public EnergiaDistribuidaContext(DbContextOptions<EnergiaDistribuidaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<ConsumoPorTramo> ConsumoPorTramos { get; set; }

    public virtual DbSet<CostosPorTramo> CostosPorTramos { get; set; }

    public virtual DbSet<PerdidasPorTramo> PerdidasPorTramos { get; set; }

    public virtual DbSet<TiposCliente> TiposClientes { get; set; }

    public virtual DbSet<Tramo> Tramos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.GananciaOperdida).HasColumnName("GananciaOPerdida");

            entity.HasOne(d => d.TipoCliente).WithMany()
                .HasForeignKey(d => d.TipoClienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Clientes_TiposClientes");

            entity.HasOne(d => d.Tramo).WithMany()
                .HasForeignKey(d => d.TramoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Clientes_Tramos");
        });

        modelBuilder.Entity<ConsumoPorTramo>(entity =>
        {
            entity.ToTable("ConsumoPorTramo");

            entity.Property(e => e.ConsumoComercial).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ConsumoIndustrial).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ConsumoResidencial).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Fecha).HasColumnType("date");
            entity.Property(e => e.Tramo)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CostosPorTramo>(entity =>
        {
            entity.ToTable("CostosPorTramo");

            entity.Property(e => e.Fecha).HasColumnType("date");
            entity.Property(e => e.Tramo)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PerdidasPorTramo>(entity =>
        {
            entity.ToTable("PerdidasPorTramo");

            entity.Property(e => e.PerdidaComercial).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PerdidaIndustrial).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PerdidaResidencial).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Fecha).HasColumnType("date");
            entity.Property(e => e.Tramo)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
