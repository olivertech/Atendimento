using System;
using System.Collections.Generic;
using Dommel;
using System.Linq;
using Atendimento.Repository.Repositories.Base;
//using Dapper.Contrib.Extensions;

namespace Atendimento.Repository.Repositories
{
    /// <summary>
    /// Classe base com recuperação da string de conexão de banco
    /// e criação de uma nova conexão de banco
    /// </summary>
    public class RepositoryBaseOLD<T> : ConnectionBase where T : class
    {
        protected T result;

        public IEnumerable<T> GetAll()
        {
            IEnumerable<T> list;

            try
            {
                using (var conn = CreateConnection())
                {
                    list = conn.GetAll<T>().ToList();
                }

                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public T Get(int id)
        {
            try
            {
                using (var conn = CreateConnection())
                {
                    result = conn.Get<T>(id);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Insert(T obj)
        {
            int identity;

            try
            {
                using (var conn = CreateConnection())
                {
                    identity = int.Parse(conn.Insert(obj).ToString());
                }

                return identity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Insert(IEnumerable<T> list)
        {
            int result = 0;

            var conn = CreateConnection();

            using (var transaction = conn.BeginTransaction())
            {
                try
                {
                    foreach (var item in list)
                    {
                        conn.Insert(item, transaction);
                        result++;
                    }

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    result = 0;
                }
            }

            return result;
        }

        public bool Update(T obj)
        {
            bool result;

            try
            {
                using (var conn = CreateConnection())
                {
                    result = conn.Update(obj);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(IEnumerable<T> list)
        {
            bool result;

            var conn = CreateConnection();

            using (var transaction = conn.BeginTransaction())
            {
                try
                {
                    foreach (var item in list)
                    {
                        conn.Update(item, transaction);
                    }

                    transaction.Commit();
                    result = true;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    result = false;
                }
            }

            return result;
        }

        public bool Delete(T obj)
        {
            bool result;

            try
            {
                using (var conn = CreateConnection())
                {
                    result = conn.Delete(obj);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(IEnumerable<T> list)
        {
            bool result;

            var conn = CreateConnection();

            using (var transaction = conn.BeginTransaction())
            {
                try
                {
                    foreach (var item in list)
                    {
                        conn.Delete(item, transaction);
                    }

                    transaction.Commit();
                    result = true;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    result = false;
                }
            }

            return result;
        }
    }
}
