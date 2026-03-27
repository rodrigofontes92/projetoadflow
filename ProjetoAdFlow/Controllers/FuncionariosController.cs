using AdFlow.Data;
using AdFlow.Models;
using AdFlow.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "Administrador")]
/*
 * Controller responsável pela gestão de funcionários do sistema.
 * 
 * Os funcionários são representados pelos utilizadores do ASP.NET Identity armazenados na tabela AspNetUsers.
 */
public class FuncionariosController : Controller
{
    private readonly ApplicationDbContext db;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public FuncionariosController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        db = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<IActionResult> Index()
    {
        var users = _userManager.Users.ToList();

        /*
         * Cria uma lista auxiliar com os dados + cargo.
         * Criado com ajuda de IA
         */
        var lista = new List<(ApplicationUser user, string role)>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);

            /*
             * Considerando 1 cargo por utilizador.
             */
            var role = roles.FirstOrDefault() ?? "Sem cargo";

            lista.Add((user, role));
        }

        return View(lista);
    }

    /*
    * GET: Editar funcionário
    */
    public async Task<IActionResult> Edit(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user == null)
            return NotFound();

        /*
         * Obtém o cargo atual do utilizador.
         */
        var roles = await _userManager.GetRolesAsync(user);
        var currentRole = roles.FirstOrDefault();

        /*
         * Lista todos os cargos para a combobox.
         * Criado com ajuda de IA
         */
        ViewData["Roles"] = new SelectList(
            _roleManager.Roles.Select(r => r.Name),
            currentRole
        );

        return View(user);
    }

    /*
    * POST: Editar funcionário
    * Alguns trechos criados com ajuda de IA
    */
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, ApplicationUser funcionario, string role)
    {
        if (id != funcionario.Id)
            return NotFound();

        var userDb = await _userManager.FindByIdAsync(id);

        if (userDb == null)
            return NotFound();

        /*
         * Atualiza os dados do utilizador.
         */
        userDb.Nome = funcionario.Nome;
        userDb.NIF = funcionario.NIF;
        userDb.Endereco = funcionario.Endereco;
        userDb.Email = funcionario.Email;
        userDb.UserName = funcionario.Email;

        await _userManager.UpdateAsync(userDb);

        /*
         * Atualiza o cargo (role)
         */
        var rolesAtuais = await _userManager.GetRolesAsync(userDb);

        /*
         * Remove cargos antigos
         */
        await _userManager.RemoveFromRolesAsync(userDb, rolesAtuais);

        /*
         * Adiciona o novo cargo
         */
        await _userManager.AddToRoleAsync(userDb, role);

        return RedirectToAction(nameof(Index));
    }

    /*
    * Confirmação de remoção de funcionário.
    */
    public async Task<IActionResult> Delete(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var funcionario = await db.Users
            .FirstOrDefaultAsync(m => m.Id == id);

        if (funcionario == null)
        {
            return NotFound();
        }

        return View(funcionario);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var funcionario = await db.Users.FindAsync(id);

        if (funcionario != null)
        {
            db.Users.Remove(funcionario);
        }

        await db.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    private bool FuncionarioExiste(string id)
    {
        return db.Users.Any(e => e.Id == id);
    }
}