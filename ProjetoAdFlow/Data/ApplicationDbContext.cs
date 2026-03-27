using AdFlow.Models;
// Importa a classe ApplicationUser criada no projeto
using AdFlow.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AdFlow.Data
{
    /*
     * Classe: ApplicationDbContext
     * ------------------------------------------------------------------
     * Esta classe representa o contexto de dados da aplicação.
     * 
     * Ela é responsável por:
     * 
     * - Gerir a comunicação com a base de dados
     * - Mapear entidades para tabelas
     * - Aplicar migrations
     * - Integrar o ASP.NET Identity com o Entity Framework Core
     * 
     * Ao herdar de IdentityDbContext<ApplicationUser>,
     * estamos a informar ao Identity que o utilizador padrão
     * do sistema agora é a classe ApplicationUser,
     * e não mais IdentityUser.
     * 
     * Isso permite que os campos personalizados (Nome, NIF, Endereco)
     * passem a fazer parte da tabela AspNetUsers.
     */
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        /*
         * ==========================================================
         * DbSets do Domínio
         * ==========================================================
         * 
         * Cada DbSet representa uma tabela na base de dados
         */

        /*
         * Tabela de Clientes
         */
        public DbSet<Cliente> Clientes { get; set; }


        /*
         * Tabela de Campanhas
         */
        public DbSet<Campanha> Campanhas { get; set; }


        /*
         * Tabela de Materiais publicitários
         */
        public DbSet<Material> Materiais { get; set; }


        /*
         * Tabela de histórico de ações das campanhas
         */
        public DbSet<FluxoCampanha> FluxosCampanha { get; set; }
    }

    
}
