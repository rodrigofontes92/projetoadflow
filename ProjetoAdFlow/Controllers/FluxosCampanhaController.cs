using AdFlow.Data;
using AdFlow.Models;
using AdFlow.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdFlow.Controllers
{
    [Authorize(Roles = "Administrador,AnalistaOn,AnalistaOff,Designer,Redator")]
    public class FluxosCampanhaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public FluxosCampanhaController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: FluxosCampanha
        // Código feito com ajuda de IA
        public async Task<IActionResult> Index(int? campanhaId)
        {
            var fluxos = _context.FluxosCampanha
                .Include(f => f.Campanha)
                .Include(f => f.Funcionario)
                .Include (f => f.Material)
                .AsQueryable();

            if (campanhaId.HasValue)
                fluxos = fluxos.Where(f => f.CampanhaId == campanhaId);

            ViewBag.Campanhas = _context.Campanhas.ToList();

            return View(await fluxos.ToListAsync());
        }
        //public async Task<IActionResult> Index()
        //{
        //    var applicationDbContext = _context.FluxosCampanha.Include(f => f.Campanha).Include(f => f.Funcionario).Include(f => f.Material);
        //    return View(await applicationDbContext.ToListAsync());
        //}

        // GET: FluxosCampanha/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fluxoCampanha = await _context.FluxosCampanha
                .Include(f => f.Campanha)
                .Include(f => f.Funcionario)
                .Include(f => f.Material)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fluxoCampanha == null)
            {
                return NotFound();
            }

            return View(fluxoCampanha);
        }

        // GET: FluxosCampanha/Create
        public IActionResult Create(int campanhaId)
        {
            ViewData["CampanhaId"] = new SelectList(_context.Campanhas, "Id", "Nome", campanhaId);
            ViewData["MaterialId"] = new SelectList(_context.Materiais, "Id", "Nome");
            return View();
        }

        // POST: FluxosCampanha/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Descricao,CampanhaId,MaterialId")] FluxoCampanha fluxoCampanha)
        {
            if (ModelState.IsValid)
            {
                /*
                 * Obtém o utilizador autenticado.
                 */
                var user = await _userManager.GetUserAsync(User);

                /*
                 * Define automaticamente os dados de auditoria.
                 */
                fluxoCampanha.FuncionarioId = user.Id;
                fluxoCampanha.DataCriacao = DateTime.Now;

                _context.Add(fluxoCampanha);
                await _context.SaveChangesAsync();

                // Código criado com ajuda da IA
                return RedirectToAction("Details", "Campanhas", new { id = fluxoCampanha.CampanhaId });
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
            }

            ViewData["CampanhaId"] = new SelectList(_context.Campanhas, "Id", "Nome");
            ViewData["MaterialId"] = new SelectList(_context.Materiais, "Id", "Nome");

            return View(fluxoCampanha);

        }

        // GET: FluxosCampanha/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fluxoCampanha = await _context.FluxosCampanha.FindAsync(id);
            if (fluxoCampanha == null)
            {
                return NotFound();
            }
            ViewData["CampanhaId"] = new SelectList(_context.Campanhas, "Id", "Nome", fluxoCampanha.CampanhaId);
            ViewData["MaterialId"] = new SelectList(_context.Materiais, "Id", "Nome", fluxoCampanha.MaterialId);
            return View(fluxoCampanha);
        }

        /*
        * Atualiza um registo existente no fluxo da campanha.
        * Apenas os campos Descricao e MaterialId podem ser alterados.
        */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descricao,MaterialId")] FluxoCampanha fluxoCampanha)
        {
            if (id != fluxoCampanha.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    /*
                     * Obtém o registo original da base de dados.
                     */
                    var fluxoDb = await _context.FluxosCampanha.FindAsync(id);

                    if (fluxoDb == null)
                    {
                        return NotFound();
                    }

                    /*
                     * Atualiza apenas os campos permitidos.
                     */
                    fluxoDb.Descricao = fluxoCampanha.Descricao;
                    fluxoDb.MaterialId = fluxoCampanha.MaterialId;

                    /*
                     * Guarda as alterações.
                     */
                    await _context.SaveChangesAsync();

                    /*
                     * Após editar, volta para a página Details da campanha.
                     * Código criado com ajuda de IA
                     */
                    return RedirectToAction("Details", "Campanhas", new { id = fluxoDb.CampanhaId });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FluxoCampanhaExists(fluxoCampanha.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            /*
             * Caso haja erro de validação, recria os dropdowns.
             */
            ViewData["MaterialId"] = new SelectList(_context.Materiais, "Id", "Nome", fluxoCampanha.MaterialId);

            return View(fluxoCampanha);
        }

        [Authorize(Roles = "Administrador")]
        // GET: FluxosCampanha/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fluxoCampanha = await _context.FluxosCampanha
                .Include(f => f.Campanha)
                .Include(f => f.Funcionario)
                .Include(f => f.Material)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fluxoCampanha == null)
            {
                return NotFound();
            }

            return View(fluxoCampanha);
        }

        [Authorize(Roles = "Administrador")]
        // POST: FluxosCampanha/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fluxoCampanha = await _context.FluxosCampanha.FindAsync(id);
            var campanhaId = fluxoCampanha?.CampanhaId;

            if (fluxoCampanha != null)
            {
                _context.FluxosCampanha.Remove(fluxoCampanha);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Campanhas", new { id =campanhaId });
        }

        private bool FluxoCampanhaExists(int id)
        {
            return _context.FluxosCampanha.Any(e => e.Id == id);
        }
    }
}
