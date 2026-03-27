namespace AdFlow.Models.Enums
{
    /*
     * Enum: TipoCampanha
     * -------------------------------------------------------------
     * Representa o tipo de campanha publicitária.
     * 
     * Este enum é utilizado para definir se a campanha será:
     * 
     * - Online  → Campanhas digitais (Google Ads, Meta Ads, etc.)
     * - Offline → Campanhas físicas (Outdoor, Rádio, Impressos, etc.)
     * 
     * Justificativa do uso de enum:
     * - Evita uso de strings soltas
     * - Garante integridade do domínio
     * - Facilita filtros e validações
     */
    public enum TipoCampanha
    {
        Online = 1,
        Offline = 2
    }
}
