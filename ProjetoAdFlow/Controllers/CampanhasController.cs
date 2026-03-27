using AdFlow.Data;
using AdFlow.Models;
using AdFlow.Models.Identity;
using AdFlow.Models.Enums;
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
    public class CampanhasController : Controller
    {
        /*
        * Instância do UserManager responsável por
        * gerir os utilizadores do ASP.NET Identity.
        */
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public CampanhasController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Roles = "Administrador,AnalistaOn,AnalistaOff,Redator,Designer")]
        // GET: Campanhas
        public async Task<IActionResult> Index(int? clienteId)
        {
            var campanhas = _context.Campanhas
                .Include(c => c.Cliente)
                .Include(c => c.Funcionario)
                .AsQueryable();

            if (clienteId.HasValue)
                campanhas = campanhas.Where(c => c.ClienteId == clienteId);

            ViewBag.Clientes = _context.Clientes.ToList();

            return View(await campanhas.ToListAsync());
        }
        //public async Task<IActionResult> Index()
        //{
        //    var campanhas = _context.Campanhas
        //        .Include(c => c.Cliente)
        //        .Include(c => c.Funcionario);

        //    return View(await campanhas.ToListAsync());
        //}

        [Authorize(Roles = "Administrador,AnalistaOn,AnalistaOff,Redator,Designer")]
        // GET: Campanhas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            /*
            * Obtém a campanha juntamente com:
            * - Cliente
            * - Funcionário criador
            * - Fluxos associados
            */
            var campanha = await _context.Campanhas
                .Include(c => c.Cliente)
                .Include(c => c.Funcionario)
                .Include(c => c.Fluxos)
                    .ThenInclude(f => f.Funcionario)
                .Include(c => c.Fluxos)
                    .ThenInclude(f => f.Material)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (campanha == null)
            {
                return NotFound();
            }

            return View(campanha);
        }

        [Authorize(Roles = "Administrador,AnalistaOn,AnalistaOff")]
        // GET: Campanhas/Create
        public IActionResult Create()
        {
            ViewData["ClienteId"] = new SelectList(
                _context.Clientes.OrderBy(c => c.Nome),
                "Id",
                "Nome"
            );
           
            return View();
        }

        [Authorize(Roles = "Administrador,AnalistaOn,AnalistaOff")]
        /*
        * Método responsável por processar o POST do formulário de criação de campanhas.
        * 
        */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,Nome,DataCriacao,Tipo,DataInicio,DataFim,Orcamento,Prioridade,Status,ClienteId")] Campanha campanha)
        {
            if (ModelState.IsValid)
            {
                /*
                 * Obtém o utilizador autenticado no sistema.
                 */
                var user = await _userManager.GetUserAsync(User);

                /*
                 * Associa o utilizador à campanha criada.
                 * Código criado com ajuda da IA
                 */
                campanha.FuncionarioId = user.Id;

                _context.Add(campanha);

                /*
                 * Salva a campanha e gera o Id automaticamente.
                 */
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            /*
            * Caso o formulário possua erros de validação,
            * precisamos recriar os dados do dropdown antes de retornar à View.
            */
            ViewData["ClienteId"] = new SelectList(
                _context.Clientes.OrderBy(c => c.Nome),
                "Id",
                "Nome",
                campanha.ClienteId
            );

            return View(campanha);
        }

        [Authorize(Roles = "Administrador,AnalistaOn,AnalistaOff")]
        // GET: Campanhas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var campanha = await _context.Campanhas.FindAsync(id);
            if (campanha == null)
            {
                return NotFound();
            }

            /*
            * Preenche o dropdown de clientes mostrando o Nome
            * em vez da Classe do cliente.
            */
            ViewData["ClienteId"] = new SelectList(
                _context.Clientes.OrderBy(c => c.Nome),
                "Id",
                "Nome",
                campanha.ClienteId
            );
            
            return View(campanha);
        }

        [Authorize(Roles = "Administrador,AnalistaOn,AnalistaOff")]
        // POST: Campanhas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,DataCriacao,Tipo,DataInicio,DataFim,Orcamento,Prioridade,Status,ClienteId")] Campanha campanha)
        {
            /*
             * Verifica se o id recebido corresponde à campanha enviada.
             */
            if (id != campanha.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try // Criado com ajuda da IA
                {
                    /*
                     * Recupera a campanha original da base de dados
                     * para preservar o FuncionarioId do criador da campanha.
                     */
                    var campanhaOriginal = await _context.Campanhas
                        .AsNoTracking()
                        .FirstOrDefaultAsync(c => c.Id == id);

                    /*
                     * Mantém o funcionário que criou originalmente a campanha.
                     * Mesmo que outro utilizador edite a campanha, o criador permanece.
                     */
                    campanha.FuncionarioId = campanhaOriginal.FuncionarioId;

                    /*
                     * Atualiza a campanha com os novos dados.
                     */
                    _context.Update(campanha);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CampanhaExists(campanha.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                /*
                 * Redireciona para a listagem de campanhas após edição bem sucedida.
                 */
                return RedirectToAction(nameof(Index));
            }

            /*
             * Caso ocorra erro de validação, recriamos o dropdown de clientes
             * para que o formulário seja exibido novamente corretamente.
             */
            ViewData["ClienteId"] = new SelectList(
                _context.Clientes.OrderBy(c => c.Nome),
                "Id",
                "Nome",
                campanha.ClienteId
            );

            return View(campanha);
        }

        [Authorize(Roles = "Administrador")]
        // GET: Campanhas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var campanha = await _context.Campanhas
                .Include(c => c.Cliente)
                .Include(c => c.Funcionario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (campanha == null)
            {
                return NotFound();
            }

            return View(campanha);
        }

        [Authorize(Roles = "Administrador")]
        // POST: Campanhas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var campanha = await _context.Campanhas.FindAsync(id);
            if (campanha != null)
            {
                _context.Campanhas.Remove(campanha);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CampanhaExists(int id)
        {
            return _context.Campanhas.Any(e => e.Id == id);
        }
    }
}
