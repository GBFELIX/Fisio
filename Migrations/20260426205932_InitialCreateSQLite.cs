using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlanTributario.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateSQLite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Lancamentoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataLancamento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ValorBruto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TipoAtendimento = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProLabore = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImpostoDas = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Inss = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProntuarioPath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lancamentoes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Paciente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataNascimento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Telefone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sexo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cidade = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Bairro = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Profissao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnderecoResidencial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Naturalidade = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstadoCivil = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paciente", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Prontuario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PacienteId = table.Column<int>(type: "int", nullable: false),
                    LancamentoId = table.Column<int>(type: "int", nullable: false),
                    DataAtendimento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HistoriaClinica = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QueixaPrincipal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HabitosVida = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HMA = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HMP = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AntecedentesPessoais = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AntecedentesFamiliares = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TratamentosRealizados = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlanoTerapeutico = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Evolucao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApresentacaoPaciente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExamesComplementares = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UsaMedicamentos = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RealizouCirurgia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InspecaoPalpacao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NivelDor = table.Column<int>(type: "int", nullable: true),
                    PressaoArterial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Saturacao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FrequenciaCardiaca = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prontuario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prontuario_Lancamentoes_LancamentoId",
                        column: x => x.LancamentoId,
                        principalTable: "Lancamentoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Prontuario_Paciente_PacienteId",
                        column: x => x.PacienteId,
                        principalTable: "Paciente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Prontuario_LancamentoId",
                table: "Prontuario",
                column: "LancamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Prontuario_PacienteId",
                table: "Prontuario",
                column: "PacienteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Prontuario");

            migrationBuilder.DropTable(
                name: "Lancamentoes");

            migrationBuilder.DropTable(
                name: "Paciente");
        }
    }
}
