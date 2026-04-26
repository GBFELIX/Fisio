using Microsoft.EntityFrameworkCore;
using PlanTributario.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DbContext_PlanTributario
{
    public class Data_ : DbContext
    {
        public Data_(DbContextOptions<Data_> options)
            : base(options)
        {
        }

        public DbSet<PlanTributario.Models.Lancamento> Lancamento { get; set; } = default!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Lancamento>().ToTable("Lancamentoes");

            // Resolve o aviso do Decimal 
            modelBuilder.Entity<Lancamento>()
                .Property(p => p.ValorBruto)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Lancamento>()
                .Property(p => p.ProLabore)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Lancamento>()
                .Property(p => p.ImpostoDas)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Lancamento>()
                .Property(p => p.Inss)
                .HasColumnType("decimal(18,2)");
        }
        public DbSet<PlanTributario.Models.Paciente> Paciente { get; set; } = default!;
        public DbSet<PlanTributario.Models.Prontuario> Prontuario { get; set; } = default!;
    }
}
