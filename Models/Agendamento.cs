using PlanTributario.Models;
using System.ComponentModel.DataAnnotations;

public class Agendamento
{
    public int Id { get; set; }

    [Required]
    public string Titulo { get; set; } // Ex: "Consulta - João Silva"

    [Required]
    public DateTime Inicio { get; set; }

    [Required]
    public DateTime Fim { get; set; }

    public string? Cor { get; set; } // Para ela categorizar (ex: Particular vs Clínica)

    public string? Observacoes { get; set; }

    // Relacionamento opcional com Paciente
    public int? PacienteId { get; set; }
    public virtual Paciente? Paciente { get; set; }
}