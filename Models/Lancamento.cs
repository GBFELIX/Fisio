using System.ComponentModel.DataAnnotations.Schema;

namespace PlanTributario.Models
{
    public class Lancamento
    {
        public int Id { get; set; }

        [Column("DataLancamento")]
        public DateTime Data { get; set; } = DateTime.Now;
        public string Descricao { get; set; }
        public decimal ValorBruto { get; set; }
        public string TipoAtendimento { get; set; }

        public decimal ProLabore { get; set; }
        public decimal ImpostoDas { get; set; }
        public decimal Inss { get; set; }

        public decimal ValorLiquido => ValorBruto - ImpostoDas - Inss;

        public string? ProntuarioPath { get; set; }
    }
}