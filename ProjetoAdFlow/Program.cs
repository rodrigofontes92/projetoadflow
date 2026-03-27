using AdFlow.Data;
using AdFlow.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

/*
* Configuração do ASP.NET Core Identity
* ------------------------------------------------------------
* Aqui informamos ao sistema que o tipo de utilizador da aplicação
* não é mais IdentityUser padrão, mas sim ApplicationUser,
* que contém campos personalizados (Nome, NIF, Endereco).
* 
* Conta com suporte a Roles (cargos) em .AddRoles<IdentityRole>()
*/
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    /*
    * Define que não será necessária confirmação de email para login.
    */
    options.SignIn.RequireConfirmedAccount = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();

/*
 * Adição do UseAuthentication() para que o sistema reconheça o cookie
 * corretamente e evitar que o [Authorize] não funcione como esperado
 */
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

/*
 * Criação automática dos cargos do sistema na base de dados.
 */
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedRoles.CreateRoles(services);
}

app.Run();
