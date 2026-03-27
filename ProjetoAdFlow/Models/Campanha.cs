using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AdFlow.Models.Enums;
using AdFlow.Models.Identity;

namespace AdFlow.Models
{
    /*
     * Entidade: Campanha
     * ------------------------------------------------------------------
     * Representa uma campanha publicitária no sistema AdFlow.
     * 
     * Esta é a entidade central do domínio.
     * 
     * Cada campanha:
     * - Pertence a um Cliente
     * - Possui um Criador (ApplicationUser)
     * - Possui um Tipo (Online/Offline)
     * - Possui Prioridade
     * - Possui Status
     * - Pode ter múltiplos registos no FluxoCampanha
     */

    public class Campanha
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Nome { get; set; } = string.Empty;

        /*
         * Data de criação.
         * Será preenchida automaticamente no momento da criação (.Now)
         */
        [Display(Name = "Data Criação")]
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        /*
         * Tipo da campanha (Online ou Offline).
         * Enum
         * 
         * Campo brigatório
         */
        [Required]
        public TipoCampanha Tipo { get; set; }

        [Display(Name = "Data Início")]
        public DateTime DataInicio { get; set; }

        [Display(Name = "Data Fim")]
        public DateTime DataFim { get; set; }

        [Display(Name = "Orçamento")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Orcamento { get; set; }

        /*
         * Prioridade da campanha (enum)
         * Campo obrigatório
         */
        [Required]
        public Prioridade Prioridade { get; set; }

        /*
         * Status atual da campanha (enum)
         * Campo obrigatório
         */
        [Required]
        public StatusCampanha Status { get; set; }

        /*
         * Chave estrangeira para Cliente.
         */
        [ForeignKey("Cliente")]
        public int ClienteId { get; set; }

        /*
         * Propriedade de navegação.
         * N Campanhas -> 1 Cliente
         */
        public virtual Cliente? Cliente { get; set; }

        public string? FuncionarioId { get; set; }

        /*
         * Propriedade de navegação.
         * N Campanhas -> 1 Utilizador (criador)
         */
        public virtual ApplicationUser? Funcionario { get; set; }

        /*
         * Uma campanha pode possuir múltiplos registos
         * de histórico no FluxoCampanha.
         */
        public virtual ICollection<FluxoCampanha> Fluxos { get; set; } = new List<FluxoCampanha>();
    }
}
