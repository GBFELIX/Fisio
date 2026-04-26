using DbContext_PlanTributario;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PlanTributario.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlanTributario.Controllers
{
    public class ProntuariosController : Controller
    {
        private readonly Data_ _context;

        public ProntuariosController(Data_ context)
        {
            _context = context;
        }

        public IActionResult Criar(int lancamentoId, int pacienteId)
        {
            var lancamento = _context.Lancamento.Find(lancamentoId);
            var paciente = _context.Paciente.Find(pacienteId);

            if (lancamento == null) return NotFound();

            var model = new Prontuario
            {
                LancamentoId = lancamentoId,
                PacienteId = pacienteId,

            };
            ViewBag.NomePaciente = paciente.Nome;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Salvar(Prontuario model)
        {
            // Log para depuração
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine("ERRO DE VALIDAÇÃO: " + error.ErrorMessage);
                }

                return View("Criar", model);
            }

            _context.Prontuario.Add(model);
            await _context.SaveChangesAsync();


            return RedirectToAction("Index", "Lancamentoes");
        }
        public async Task<IActionResult> Detalhes(int id)
        {
            var prontuario = await _context.Prontuario
                .Include(p => p.Paciente)
                .Include(p => p.Lancamento)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (prontuario == null) return NotFound();

            return View(prontuario);
        }
        public async Task<IActionResult> Index()
        {

            var prontuarios = await _context.Prontuario
                .Include(p => p.Paciente)
                .Include(p => p.Lancamento)
                .OrderByDescending(p => p.Id)
                .ToListAsync();
            return View(prontuarios);
        }
        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null) return NotFound();

            var prontuario = await _context.Prontuario
                .Include(p => p.Paciente)
                .Include(p => p.Lancamento)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (prontuario == null) return NotFound();

            return View(prontuario);
        }

        // POST: Prontuarios/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, Prontuario model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Prontuario.Any(e => e.Id == model.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
    }
}
