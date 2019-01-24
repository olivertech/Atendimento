using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Atendimento.Business.Interfaces.Interfaces;
using Atendimento.Entities.Entities;

namespace Atendimento.Business.Interfaces.Partials
{
    public partial interface ICategoriaBusiness : IBusiness<Categoria>
    {
        IEnumerable<Categoria> GetItems(CommandType commandType, string sql, object parameters = null);
    }
}
