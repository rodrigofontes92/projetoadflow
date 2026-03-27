using AdFlow.Models.Enums;
using AdFlow.Models.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdFlow.Models
{
    /*
     * Entidade: Material
     * ------------------------------------------------------------------
     * Representa um material publicitário associado a campanhas.
     * 
     * Pode ser:
     * - Texto (conteúdo produzido por redator)
     * - Criativo (arte gráfica produzida por designer)
     * 
     * Os ficheiros físicos serão armazenados em wwwroot/uploads
     * 
     * A base de dados armazenará apenas o caminho do ficheiro.
     */

    public class Material
    {
        /*
         * Identificador único do material
         * Chave primária.
         */
        public int Id { get; set; }

        /*
         * Nome identificador do material.
         * Exemplo: "Copy Black Friday", "Banner 1200x628"
         * Campo obrigatório
         */
        [Required]
        [MaxLength(150)]
        public string Nome { get; set; } = string.Empty;

        /*
         * Tipo do material (Texto ou Criativo).
         * 
         * Utiliza enum para manter integridade de domínio
         * e facilitar validações de permissões.
         */
        [Required]
        public TipoMaterial Tipo { get; set; }

        /*
         * Caminho físico do ficheiro armazenado no servidor.
         * 
         * Exemplo:
         * uploads/abc123.png
         * 
         * Apenas o caminho é guardado na BD,
         * nunca o ficheiro em si.
         */
        [Required]
        [Display(Name = "Caminho Ficheiro")]
        public string CaminhoArquivo { get; set; } = string.Empty;

        /*
         * Data de criação do material.
         * 
         * Preenchida automaticamente no momento do upload.
         */
        [Display(Name = "Data Criação")]
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        /*
         * Chave estrangeira para o utilizador que criou o material.
         */
        [Display(Name = "Funcionário")]
        [ForeignKey("Funcionario")]
        public string? FuncionarioId { get; set; }

        /*
         * Navegação para o funcionário criador do material.
         */
        public ApplicationUser? Funcionario { get; set; }

        /*
         * Material tem relacionamento com FluxoCampanha.
         */ 
        public virtual ICollection<FluxoCampanha> Fluxos { get; set; } = new List<FluxoCampanha>();
    }
}
