namespace Atendimento.Entities.Enums
{
    public static class Tipos
    {
        public enum Login : int
        {
            Atendimento = 1,
            Cliente
        }

        public enum TipoConsulta: int
        {
            All = 1,
            Paged = 2,
            Count
        }
    }
}
