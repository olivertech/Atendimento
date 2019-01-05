using Atendimento.Entities.Enums;

namespace Atendimento.Entities.Responses
{
    /// <summary>
    /// Classe de resposta ao login realizado, retornando a 
    /// classe preenchida com os dados do referido usuário
    /// autenticado (que pode ser um atendente ou cliente)
    /// e o tipo desse usuário autenticado
    /// </summary>
    public class LoginResponse
    {
        /// <summary>
        /// Propriedade que receber ou um objeto do tipo AtendenteEmpresa ou UsuarioCliente
        /// </summary>
        public object Usuario { get; set; }

        /// <summary>
        /// Define o tipo de usuário logado
        /// </summary>
        public string TipoUsuario { get; set; }

        /// <summary>
        /// Define o token retornado
        /// </summary>
        public string Token { get; set; }
    }
}