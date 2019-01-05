using System;
using System.Collections.Generic;
using System.Linq;
using Atendimento.Entities.Entities;
using Atendimento.Repository.Interfaces.Interfaces;
using Atendimento.Repository.Repositories.Base;
using Dommel;

namespace Atendimento.Repository.Repositories
{
    public class UsuarioClienteRepository : RepositoryBase<UsuarioCliente>, IUsuarioClienteRepository
    {
        public IEnumerable<UsuarioCliente> GetAllById(int idCliente)
        {
            List<UsuarioCliente> result = null;
            string sql = string.Empty;

            try
            {
                using (var conn = CreateConnection())
                {
                    result = conn.Select<UsuarioCliente>(q => q.IdCliente == idCliente).ToList();
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
