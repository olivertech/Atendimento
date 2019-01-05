using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Atendimento.Entities.Entities;
using Atendimento.Repository.Interfaces.Interfaces.Base;
using Dommel;

namespace Atendimento.Repository.Repositories.Base
{
    /// <summary>
    /// Classe abstrata genérica de CRUD herdada por todas as classes de repository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class RepositoryBase<T> : ConnectionBase, IRepositoryBase<T> where T : BaseEntity
    {
        public virtual T GetById(Int32 id)
        {
            try
            {
                using (var conn = CreateConnection())
                {
                    return conn.Get<T>(id);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public virtual IEnumerable<T> GetAll()
        {
            IEnumerable<T> list = new List<T>();

            try
            {
                using (var conn = CreateConnection())
                {
                    list = conn.GetAll<T>();
                }

                return list;
            }
            catch (Exception)
            {
                return list;
            }
        }

        public virtual IEnumerable<T> GetList(Expression<Func<T, bool>> predicate)
        {
            IEnumerable<T> list = new List<T>();

            try
            {
                using (var conn = CreateConnection())
                {
                    list = conn.Select(predicate);
                }

                return list;
            }
            catch (Exception)
            {
                return list;
            }
        }

        public virtual void Insert(ref T entity)
        {
            int identity;

            try
            {
                using (var conn = CreateConnection())
                {
                    identity = int.Parse(conn.Insert(entity).ToString());
                }

                entity = GetById(identity);
            }
            catch (Exception ex)
            {
                throw new Exception("Registro não foi incluído - " + ex.Message);
            }
        }

        public virtual int Insert(IEnumerable<T> list)
        {
            try
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
            catch (Exception)
            {
                return 0;
            }
        }

        public virtual bool Update(T entity)
        {
            try
            {
                using (var conn = CreateConnection())
                {
                    return conn.Update(entity);
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public virtual bool Update(IEnumerable<T> list)
        {
            try
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
            catch (Exception)
            {
                return false;
            }
        }

        public virtual bool Delete(Int32 id)
        {
            try
            {
                using (var conn = CreateConnection())
                {
                    var entity = GetById(id);

                    if (entity == null) throw new Exception("Registro não encontrado");

                    return conn.Delete(entity);
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public virtual bool Delete(IEnumerable<T> list)
        {
            try
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
            catch (Exception)
            {
                return false;
            }
        }

        public T Get(int id)
        {
            throw new NotImplementedException();
        }
    }
}
