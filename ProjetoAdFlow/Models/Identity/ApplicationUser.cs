using Microsoft.AspNetCore.Identity;

namespace AdFlow.Models.Identity
{
    /*
     * Classe: ApplicationUser
     * -------------------------------------------------------------
     * Esta classe representa o utilizador autenticado do sistema AdFlow.
     * 
     * Ela herda da classe IdentityUser, que já contém todos os campos
     * necessários para autenticação e segurança, tais como:
     * 
     * - Id
     * - UserName
     * - Email
     * - PasswordHash
     * - PhoneNumber
     * - SecurityStamp
     * - EmailConfirmed
     * - entre outros
     * 
     * Através da herança, estamos a estender a estrutura padrão do Identity
     * para incluir informações adicionais relevantes ao domínio do projeto.
     * 
     * Decisão estrutural:
     * -------------------------------------------------------------
     * Optou-se por integrar o "Funcionário" diretamente ao Identity,
     * evitando a criação de uma entidade separada para dados pessoais
     * 
     * Essa abordagem:
     * - Simplifica o modelo;
     * - Mantém compatibilidade com o Identity
     * - Evita duplicação de informação
     * - Facilita relacionamentos futuros (ex: Campanha -> Criador)
     */
    public class ApplicationUser : IdentityUser
    {
        public string Nome { get; set; } = string.Empty;

        /*
         * Número de Identificação Fiscal (NIF).
         * 
         * Tipo: string (e não int)
         * 
         * Justificativa:
         * - É um identificador, não um valor para cálculo.
         * - Pode conter zeros à esquerda
         * - Pode sofrer alterações de formato no futuro
         */
        public string NIF { get; set; } = string.Empty;
        public string Endereco {  get; set; } = string.Empty;
    }
}
