using System.ComponentModel.DataAnnotations;

namespace PlanTributario.Models
{
    public class Paciente
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [Display(Name = "Nome Completo")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O CPF é obrigatório")]
        [Display(Name = "CPF")]
        public string? Cpf { get; set; } = string.Empty;

        [Display(Name = "Data de Nascimento")]
        [DataType(DataType.Date)]
        public DateTime? DataNascimento { get; set; }

        public string? Telefone { get; set; }

        public string? Sexo { get; set; }

        public string? Cidade { get; set; }

        public string? Bairro { get; set; }

        [Display(Name = "Profissão")]
        public string? Profissao { get; set; }

        [Display(Name = "Endereço Residencial")]
        public string? EnderecoResidencial { get; set; }

        public string? Naturalidade { get; set; }

        [Display(Name = "Estado Civil")]
        public string? EstadoCivil { get; set; }
        [Display(Name = "Data de Cadastro")]
        public DateTime? DataCadastro { get; set; }
    }
}