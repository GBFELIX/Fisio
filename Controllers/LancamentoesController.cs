using DbContext_PlanTributario;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlanTributario.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanTributario.Controllers
{
    public class LancamentoesController : Controller
    {
        private readonly Data_ _context;


        public LancamentoesController(Data_ context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> BaixarExcel(int[] Selecionados)
        {
            if (Selecionados == null || Selecionados.Length == 0)
                return BadRequest("Nenhum item selecionado.");

            // await para buscar os dados
            var itensSelecionados = await _context.Lancamento
                .Where(l => Selecionados.Contains(l.Id))
                .ToListAsync();

            var csv = new StringBuilder();

            // Cabeçalho
            csv.AppendLine("Data;Descricao;Valor Bruto;Pro-Labore");

            foreach (var i in itensSelecionados)
            {
                //Formatação de valores numéricos para evitar problemas de vírgula/ponto
                csv.AppendLine($"{i.Data:dd/MM/yyyy};{i.Descricao};{i.ValorBruto:N2};{i.ProLabore:N2}");
            }

            var fileName = $"Relatorio_Tributario_{DateTime.Now:yyyyMMdd}.csv";


            var data = Encoding.UTF8.GetBytes(csv.ToString());
            var result = Encoding.UTF8.GetPreamble().Concat(data).ToArray();

            return File(result, "text/csv", fileName);
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Lancamento.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var lancamento = await _context.Lancamento
                .FirstOrDefaultAsync(m => m.Id == id);

            if (lancamento == null) return NotFound();

            return View(lancamento);
        }

        public IActionResult Create()
        {
            ModelState.Remove("ProntuarioPath");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Data,Descricao,ValorBruto,TipoAtendimento,ProntuarioPath")] Lancamento lancamento, IFormFile arquivoProntuario)
        {
            ModelState.Remove("ProntuarioPath");
            ModelState.Remove("arquivoProntuario");
            if (ModelState.IsValid)
            {
                if (arquivoProntuario != null && arquivoProntuario.Length > 0)
                {
                    // Define o caminho da pasta
                    string folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                    // Gera um nome único para o arquivo (evita sobrescrever arquivos de pacientes diferentes)
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(arquivoProntuario.FileName);
                    string filePath = Path.Combine(folder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await arquivoProntuario.CopyToAsync(stream);
                    }

                    lancamento.ProntuarioPath = fileName;
                }
                // Cálculo automático dos impostos (Fator R)
                lancamento.ProLabore = lancamento.ValorBruto * 0.28m;
                lancamento.ImpostoDas = lancamento.ValorBruto * 0.06m;
                lancamento.Inss = lancamento.ProLabore * 0.11m;

                _context.Add(lancamento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(lancamento);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var lancamento = await _context.Lancamento.FindAsync(id);
            if (lancamento == null) return NotFound();

            return View(lancamento);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Data,Descricao,ValorBruto,TipoAtendimento")] Lancamento lancamento, IFormFile? novoArquivo)
        {
            if (id != lancamento.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (novoArquivo != null && novoArquivo.Length > 0)
                    {
                        string folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                        // Deletar o arquivo antigo se ele existir
                        if (!string.IsNullOrEmpty(lancamento.ProntuarioPath))
                        {
                            string oldPath = Path.Combine(folder, lancamento.ProntuarioPath);
                            if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);
                        }

                        // Salvar o novo arquivo
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(novoArquivo.FileName);
                        string filePath = Path.Combine(folder, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await novoArquivo.CopyToAsync(stream);
                        }

                        // Atualiza o caminho no objeto que será salvo
                        lancamento.ProntuarioPath = fileName;
                    }

                    lancamento.ProLabore = lancamento.ValorBruto * 0.28m;
                    lancamento.ImpostoDas = lancamento.ValorBruto * 0.06m;
                    lancamento.Inss = lancamento.ProLabore * 0.11m;

                    _context.Update(lancamento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LancamentoExists(lancamento.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(lancamento);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var lancamento = await _context.Lancamento
                .FirstOrDefaultAsync(m => m.Id == id);

            if (lancamento == null) return NotFound();

            return View(lancamento);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lancamento = await _context.Lancamento.FindAsync(id);
            if (lancamento != null)
            {
                _context.Lancamento.Remove(lancamento);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LancamentoExists(int id)
        {
            return _context.Lancamento.Any(e => e.Id == id);
        }
        [HttpPost]
        public async Task<IActionResult> ExportarCSV(int[] ids)
        {
            // 1. Busca apenas os lançamentos que foram marcados no checkbox
            var dados = await _context.Lancamento
                .Where(l => ids.Contains(l.Id))
                .ToListAsync();

            if (dados.Count == 0) return NotFound();

            // 2. Monta o cabeçalho do CSV
            var csv = new StringBuilder();
            csv.AppendLine("Data;Descricao;Valor Bruto;Pro-Labore;DAS;INSS;Valor Liquido");

            // 3. Preenche as linhas com os dados
            foreach (var item in dados)
            {
                csv.AppendLine($"{item.Data:dd/MM/yyyy};{item.Descricao};{item.ValorBruto};{item.ProLabore};{item.ImpostoDas};{item.Inss};{item.ValorLiquido}");
            }

            // 4. Transforma em bytes e retorna como arquivo para download
            var bytes = Encoding.UTF8.GetBytes(csv.ToString());
            return File(bytes, "text/csv", $"Relatorio_Tributario_{DateTime.Now:yyyyMMdd}.csv");
        }
        public async Task<IActionResult> Relatorio(int? mes, int? ano)
        {
            int mesConsulta = mes ?? DateTime.Now.Month;
            int anoConsulta = ano ?? DateTime.Now.Year;

            var lancamentos = await _context.Lancamento
                .Where(l => l.Data.Month == mesConsulta && l.Data.Year == anoConsulta)
                .OrderBy(l => l.Data)
                .ToListAsync();

            ViewBag.Mes = mesConsulta;
            ViewBag.Ano = anoConsulta;

            return View(lancamentos);
        }
    }
}