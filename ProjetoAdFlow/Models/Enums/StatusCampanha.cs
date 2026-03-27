namespace AdFlow.Models.Enums
{
    /*
     * Enum: StatusCampanha
     * -------------------------------------------------------------
     * Representa o estado atual de uma campanha.
     * 
     * Fluxo esperado:
     * 
     * Planeada → EmProducao → Concluida
     * 
     * Este enum será fundamental para:
     * - Dashboards
     * - Indicadores de progresso
     * - Controlo de fluxo operacional
     */
    public enum StatusCampanha
    {
        Planeada = 1,
        EmProducao = 2,
        Concluida = 3
    }
}
