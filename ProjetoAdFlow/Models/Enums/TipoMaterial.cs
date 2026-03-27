namespace AdFlow.Models.Enums
{
    /*
     * Enum: TipoMaterial
     * -------------------------------------------------------------
     * Define o tipo de material publicitário associado à campanha.
     * 
     * - Texto     → Conteúdo textual (.txt)
     * - Criativo  → Material gráfico (.png)
     * 
     * Este enum será utilizado para:
     * - Validação de upload
     * - Separação de permissões (Redator / Designer)
     * - Organização de ficheiros
     */
    public enum TipoMaterial
    {
        Texto = 1,
        Criativo = 2
    }
}
