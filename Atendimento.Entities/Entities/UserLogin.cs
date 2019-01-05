using Atendimento.Entities.Enums;

namespace Atendimento.Entities.Entities
{
    public class UserLogin
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Nome { get; set; }
        public Tipos.Login UserType { get; set; }
    }
}
