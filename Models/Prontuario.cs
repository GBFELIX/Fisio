using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlanTributario.Models
{
    public class Prontuario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PacienteId { get; set; }

        // Adicione o ? aqui
        [ForeignKey("PacienteId")]
        public virtual Paciente? Paciente { get; set; }

        [Required]
        public int LancamentoId { get; set; }

        // Adicione o ? aqui
        [ForeignKey("LancamentoId")]
        public virtual Lancamento? Lancamento { get; set; }

        public DateTime DataAtendimento { get; set; } = DateTime.Now;

        // --- SEÇÃO 2: AVALIAÇÃO (ANAMNESE) ---
        public string? HistoriaClinica { get; set; }
        public string? QueixaPrincipal { get; set; }
        public string? HabitosVida { get; set; }
        public string? HMA { get; set; }
        public string? HMP { get; set; }
        public string? AntecedentesPessoais { get; set; }
        public string? AntecedentesFamiliares { get; set; }
        public string? TratamentosRealizados { get; set; }

        // --- SEÇÃO 3 E 4: PLANO E EVOLUÇÃO ---
        public string? PlanoTerapeutico { get; set; }
        public string? Evolucao { get; set; }
        public string? ApresentacaoPaciente { get; set; }
        public string? ExamesComplementares { get; set; }
        public string? UsaMedicamentos { get; set; }
        public string? RealizouCirurgia { get; set; }
        public string? InspecaoPalpacao { get; set; }

        public int? NivelDor { get; set; }
        public string? PressaoArterial { get; set; }
        public string? Saturacao { get; set; }
        public string? FrequenciaCardiaca { get; set; }
    }
}