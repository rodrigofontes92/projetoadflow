using AdFlow.Data;
using AdFlow.Models;
using AdFlow.Models.Enums;
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
    [Authorize(Roles = "Administrador,Designer,Redator,AnalistaOn,AnalistaOff")]
    public class MateriaisController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MateriaisController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Materiais (Criado com ajuda de IA)
        public async Task<IActionResult> Index(TipoMaterial? tipo)
        {
            var materiais = _context.Materiais
                .Include(m => m.Funcionario)
                .AsQueryable();

            if (tipo.HasValue)
                materiais = materiais.Where(m => m.Tipo == tipo.Value);

            return View(await materiais.ToListAsync());

        }

        // GET: Materiais/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var material = await _context.Materiais
                .FirstOrDefaultAsync(m => m.Id == id);
            if (material == null)
            {
                return NotFound();
            }

            return View(material);
        }

        [Authorize(Roles = "Administrador,Designer,Redator")]
        // GET: Materiais/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Materiais/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Tipo,CaminhoArquivo,DataCriacao,FuncionarioId")] Material material)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                material.FuncionarioId = user.Id;
                material.DataCriacao = DateTime.Now;

                _context.Add(material);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(material);
        }

        [Authorize(Roles = "Administrador,Designer,Redator")]
        // GET: Materiais/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var material = await _context.Materiais.FindAsync(id);
            if (material == null)
            {
                return NotFound();
            }
            return View(material);
        }

        [Authorize(Roles = "Administrador,Designer,Redator")]
        /*
        * Método responsável por atualizar um material existente.
        * 
        * O FuncionarioId não é alterado durante a edição,
        * pois representa o utilizador que criou originalmente o material.
        */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,Nome,Tipo,CaminhoArquivo,DataCriacao")] Material material)
        {
            if (id != material.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    /*
                     * Recupera o material original da base de dados
                     * para preservar o FuncionarioId do criador.
                     */
                    var materialOriginal = await _context.Materiais
                        .AsNoTracking()
                        .FirstOrDefaultAsync(m => m.Id == id);

                    /*
                     * Mantém o funcionário que criou originalmente o material.
                     */
                    material.FuncionarioId = materialOriginal.FuncionarioId;

                    /*
                     * Atualiza o material.
                     */
                    _context.Update(material);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MaterialExists(material.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(material);
        }

        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var material = await _context.Materiais
                .FirstOrDefaultAsync(m => m.Id == id);

            if (material == null)
            {
                return NotFound();
            }

            return View(material);
        }

        [Authorize(Roles = "Administrador")]
        // POST: Materiais/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var material = await _context.Materiais.FindAsync(id);
            if (material != null)
            {
                _context.Materiais.Remove(material);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MaterialExists(int id)
        {
            return _context.Materiais.Any(e => e.Id == id);
        }
    }
}
