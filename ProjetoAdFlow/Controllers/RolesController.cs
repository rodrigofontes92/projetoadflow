using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

/*
 * Classe controller dos Roles (Cargos)
 * Alguns trechos de código foram criados com ajuda da IA
 * Links de consulta:
 * https://learn.microsoft.com/en-us/answers/questions/1116816/create-role-using-identity-framework
 * https://antondevtips.com/blog/how-to-customize-aspnet-core-identity-with-efcore-for-your-project-needs
 * https://learn.microsoft.com/pt-br/aspnet/core/security/authentication/identity-configuration?view=aspnetcore-10.0
 */
[Authorize(Roles = "Administrador")]
public class RolesController : Controller
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public RolesController(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public IActionResult Index()
    {
        return View(_roleManager.Roles);
    }

    /*
     * GET: Criar cargo
     */
    public IActionResult Create()
    {
        return View();
    }

    /*
     * POST: Criar cargo
     */
    [HttpPost]
    public async Task<IActionResult> Create(string name)
    {
        if (!string.IsNullOrEmpty(name))
        {
            await _roleManager.CreateAsync(new IdentityRole(name));
        }

        return RedirectToAction(nameof(Index));
    }

    /*
    * GET: Editar cargo
    */
    public async Task<IActionResult> Edit(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);

        if (role == null)
            return NotFound();

        return View(role);
    }

    /*
     * POST: Editar cargo
     */
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, IdentityRole role)
    {
        if (id != role.Id)
            return NotFound();

        var roleDb = await _roleManager.FindByIdAsync(id);

        if (roleDb == null)
            return NotFound();

        /*
         * Atualiza apenas o nome do cargo.
         */
        roleDb.Name = role.Name;

        await _roleManager.UpdateAsync(roleDb);

        return RedirectToAction(nameof(Index));
    }

    /*
    * GET: Confirmar eliminação
    */
    public async Task<IActionResult> Delete(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);

        if (role == null)
            return NotFound();

        return View(role);
    }

    /*
     * POST: Eliminar cargo
     */
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);

        if (role != null)
        {
            await _roleManager.DeleteAsync(role);
        }

        return RedirectToAction(nameof(Index));
    }
}
