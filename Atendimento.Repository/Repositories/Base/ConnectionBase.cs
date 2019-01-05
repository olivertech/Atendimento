using System;
using System.Configuration;
using System.Data.SqlClient;

namespace Atendimento.Repository.Repositories.Base
{
    /// <summary>
    /// Classe que obtem a string de conexão do arquivo de configuração e retorna uma conexão com o banco de dados
    /// </summary>
    public class ConnectionBase
    {
        public static SqlConnection CreateConnection()
        {
            if (ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString == null)
            {
                throw new Exception("Não foi encontrado a connectionstring do banco.");
            }

            var sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);

            try
            {
                sqlConnection.Open();
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao se conectar ao banco de dados.", ex);
            }

            return sqlConnection;
        }
    }
}
