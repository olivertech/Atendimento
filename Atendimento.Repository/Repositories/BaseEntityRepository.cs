using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Dapper;

namespace Atendimento.Repository.Repositories
{
    public class BaseEntityRepository<T> where T : class, new()
    {
        private readonly string queryGetAll = "SELECT * FROM #";
        private readonly string queryGetById = "SELECT * FROM # WHERE id = @id ";
        private readonly string queryCreate = @"
                                                DECLARE @InsertedRows AS TABLE (Id int);
                                                INSERT INTO # (id, nome) OUTPUT INSERTED.Id INTO @InsertedRows VALUES (@id, @nome);
                                                SELECT id FROM @InsertedRows;
                                               ";

        public BaseEntityRepository(string entidade)
        {
            queryGetAll = queryGetAll.Replace("#", entidade);
            queryGetById = queryGetById.Replace("#", entidade);
            queryCreate = queryCreate.Replace("#", entidade);
        }

        public IList<T> GetAll()
        {
            string sql = queryGetAll;

            List<T> lista = new List<T>();

            try
            {
                using (var connection = CreateConnection())
                {
                    lista = connection.Query<T>(sql).ToList();
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public T GetById(int id)
        {
            string sql = queryGetById;

            T objeto = new T();

            try
            {
                using (var connection = CreateConnection())
                {
                    objeto = connection.QueryFirstOrDefault<T>(sql, new { id = id });
                }

                return objeto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public T Create(T entity)
        //{
        //    T objeto;

        //    string sql = queryCreate;

        //    try
        //    {
        //        using (var connection = CreateConnection())
        //        {
        //            objeto = connection.QuerySingle<T>(sql, new
        //            {
        //                nome = categoria.Nome
        //            });
        //        }

        //        return objeto;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

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
