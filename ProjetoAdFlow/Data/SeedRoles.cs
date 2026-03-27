using Microsoft.AspNetCore.Identity;

namespace AdFlow.Data
{
    public static class SeedRoles
    {
        /*
         * Método responsável por criar os cargos iniciais do sistema.
         * Criado com ajuda de IA
         */
        public static async Task CreateRoles(IServiceProvider serviceProvider)
        {
            /*
             * Obtém o RoleManager do Identity.
             */
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            /*
             * Lista de cargos do sistema.
             */
            string[] roles = {
                "Administrador",
                "AnalistaOn",
                "AnalistaOff",
                "Redator",
                "Designer"
            };

            /*
             * Percorre a lista e cria os cargos caso não existam.
             */
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}
