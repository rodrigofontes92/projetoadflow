using System.ComponentModel.DataAnnotations;

namespace AdFlow.Models
{
    /*
     * Entidade: Cliente
     * ------------------------------------------------------------------
     * Representa o cliente para o qual campanhas publicitárias são criadas.
     * 
     * Um Cliente pode possuir múltiplas Campanhas.
     * 
     * Esta entidade é fundamental para:
     * - Organização comercial
     * - Segmentação de campanhas
     * - Filtros em dashboards
     * - Relatórios financeiros
     */
    public class Cliente
    {
        public int Id { get; set; }


        //Limite de 150 caracteres por segurança e performance.
        [Required]
        [MaxLength(150)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string NIF { get; set; } = string.Empty;

        [MaxLength(250)]
        public string? Endereco { get; set; }

        [EmailAddress]
        [MaxLength(150)]
        public string? Email { get; set; }

        [MaxLength(20)]
        public string? Telemovel { get; set; }

        /*
         * Classe do cliente (A, B ou C).
         * 
         * usar enum futuramente
         */
        [Required]
        [MaxLength(1)]
        public string Classe { get; set; } = "C";

        /*
         * Propriedade de navegação
         * Representa a relação Cliente 1 -> N Campanhas
         * 
         * "virtual" permite lazy loading (caso habilitado futuramente)
         */
        public virtual ICollection<Campanha> Campanhas { get; set; } = new List<Campanha>();
    }
}
