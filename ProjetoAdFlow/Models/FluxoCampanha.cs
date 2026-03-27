using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AdFlow.Models.Identity;

namespace AdFlow.Models
{
    /*
     * Entidade: FluxoCampanha
     * ------------------------------------------------------------------
     * Representa o histórico de ações realizadas em uma campanha.
     * 
     * Esta entidade é responsável por garantir rastreabilidade completa
     * dentro do sistema AdFlow.
     * 
     * Princípio arquitetónico adotado:
     * -------------------------------------------------------------
     * O histórico é imutável.
     * 
     * Cada nova ação gera um novo registo.
     * Nunca se altera um registo existente para representar
     * uma nova ação.
     * 
     * Isso garante:
     * - Auditoria confiável
     * - Transparência
     * - Integridade operacional
     */

    public class FluxoCampanha
    {
        public int Id { get; set; }

        [Display(Name = "Descrição")]
        [Required]
        [MaxLength(300)]
        public string Descricao { get; set; } = string.Empty;

        [Display(Name = "Data Criação")]
        public DateTime DataCriacao { get; set; }

        [Display(Name = "Campanha")]
        public int CampanhaId { get; set; }

        public virtual Campanha? Campanha { get; set; }

        [Display(Name = "Material")]
        public int? MaterialId { get; set; }

        public virtual Material? Material { get; set; }

        [ForeignKey("Funcionario")]
        public string? FuncionarioId { get; set; }

        public virtual ApplicationUser? Funcionario { get; set; }
    }
}