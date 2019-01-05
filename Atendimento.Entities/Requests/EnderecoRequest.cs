using System.ComponentModel.DataAnnotations;

namespace Atendimento.Entities.Requests
{
    public class EnderecoRequest
    {
        [StringLength(255)]
        public string Logradouro { get; set; }

        [StringLength(25)]
        public string NumeroLogradouro { get; set; }

        [StringLength(50)]
        public string ComplementoLogradouro { get; set; }

        [StringLength(2)]
        public string Estado { get; set; }

        [StringLength(50)]
        public string Cidade { get; set; }

        [StringLength(50)]
        public string Bairro { get; set; }

        [StringLength(9)]
        public string Cep { get; set; }

        [StringLength(50)]
        public string Latitude { get; set; }

        [StringLength(50)]
        public string Longitude { get; set; }
    }
}