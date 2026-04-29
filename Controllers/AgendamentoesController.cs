using DbContext_PlanTributario;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlanTributario.Controllers
{
    public class AgendamentoesController : Controller
    {
        private readonly Data_ _context;

        public AgendamentoesController(Data_ context)
        {
            _context = context;
        }

        // GET: Agendamentoes
        public async Task<IActionResult> Index()
        {
            var data = _context.Agendamento.Include(a => a.Paciente);
            ViewBag.Pacientes = await _context.Paciente.OrderBy(p => p.Nome).ToListAsync();
            return View(await data.ToListAsync());
        }

        // GET: Agendamentoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agendamento = await _context.Agendamento
                .Include(a => a.Paciente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (agendamento == null)
            {
                return NotFound();
            }

            return View(agendamento);
        }

        // GET: Agendamentoes/Create
        public IActionResult Create()
        {
            ViewData["PacienteId"] = new SelectList(_context.Paciente, "Id", "Nome");
            return View();
        }

        [HttpPost]
        // Remova o [ValidateAntiForgeryToken] se tiver problemas com o Fetch, 
        // ou configure o Header do JS para enviar o token.
        public async Task<IActionResult> Create([FromBody] Agendamento agendamento)
        {
            // Removemos a validação da propriedade de navegação 'Paciente' 
            // para focar apenas no PacienteId que vem do Select
            ModelState.Remove("Paciente");

            if (ModelState.IsValid)
            {
                _context.Add(agendamento);
                await _context.SaveChangesAsync();

                // Retornamos um objeto de sucesso para o JavaScript
                return Json(new { success = true, message = "Agendamento realizado!" });
            }

            // Se houver erro, retornamos o motivo para o log do navegador
            var erros = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return BadRequest(new { success = false, errors = erros });
        }

        [HttpGet]
        public async Task<JsonResult> GetAgendamentos()
        {
            var eventos = await _context.Agendamento
                .Select(a => new
                {
                    id = a.Id,
                    title = a.Titulo,
                    start = a.Inicio.ToString("yyyy-MM-ddTHH:mm:ss"),
                    end = a.Fim.ToString("yyyy-MM-ddTHH:mm:ss"),
                    color = a.Cor,
                    extendedProps = new { pacienteId = a.PacienteId }
                })
                .ToListAsync();

            return Json(eventos);
        }
        // Busca os dados para o Modal de Edição
        [HttpGet]
        public async Task<IActionResult> GetAgendamentoById(int id)
        {
            var agendamento = await _context.Agendamento.FindAsync(id);
            if (agendamento == null) return NotFound();

            return Json(new
            {
                id = agendamento.Id,
                pacienteId = agendamento.PacienteId,
                titulo = agendamento.Titulo,
                inicio = agendamento.Inicio.ToString("yyyy-MM-ddTHH:mm"),
                fim = agendamento.Fim.ToString("yyyy-MM-ddTHH:mm"),
                cor = agendamento.Cor,
                observacoes = agendamento.Observacoes
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] Agendamento agendamento)
        {
            ModelState.Remove("Paciente"); // Evita erro de validação do objeto
            if (ModelState.IsValid)
            {
                _context.Update(agendamento);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            return BadRequest();
        }

        // GET: Agendamentoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agendamento = await _context.Agendamento
                .Include(a => a.Paciente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (agendamento == null)
            {
                return NotFound();
            }

            return View(agendamento);
        }

        // POST: Agendamentoes/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var agendamento = await _context.Agendamento.FindAsync(id);
            if (agendamento == null)
            {
                return NotFound(new { success = false, message = "Agendamento não encontrado." });
            }

            _context.Agendamento.Remove(agendamento);
            await _context.SaveChangesAsync();

            // Retorna JSON para o JavaScript saber que deu certo e atualizar o calendário
            return Json(new { success = true });
        }

        private bool AgendamentoExists(int id)
        {
            return _context.Agendamento.Any(e => e.Id == id);
        }
    }
}
