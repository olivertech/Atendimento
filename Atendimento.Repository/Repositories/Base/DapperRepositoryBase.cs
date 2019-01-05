using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;

namespace Atendimento.Repository.Repositories
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DapperKey : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class DapperIgnore : Attribute
    {
    }

    public class DapperRepositoryBase : RepositoryBase
    {
        #region Standard Dapper functionality

        /// <summary>
        /// Automatic generation of SELECT statement, BUT only for simple equality criterias!
        /// Example: Select<LogItem>(new {Class = "Client"})
        /// For more complex criteria it is necessary to call GetItems method with custom SQL statement.
        /// </summary>
        protected IEnumerable<T> Select<T>(object criteria = null)
        {
            var properties = ParseProperties(criteria);
            var sqlPairs = GetSqlPairs(properties.AllNames, " AND ");
            var sql = "";

            if (sqlPairs.Length > 0)
                sql = string.Format("SELECT * FROM [{0}] WHERE {1}", typeof(T).Name, sqlPairs);
            else
                sql = string.Format("SELECT * FROM [{0}]", typeof(T).Name);

            try
            {
                return GetItems<T>(CommandType.Text, sql, properties.AllPairs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Example: GetBySql<Activity>("SELECT * FROM Activities WHERE Id = @activityId", new {activityId = 15}).FirstOrDefault();
        protected IEnumerable<T> GetItems<T>(CommandType commandType, string sql, object parameters = null)
        {
            try
            {
                using (var connection = CreateConnection())
                {
                    return connection.Query<T>(sql, parameters, commandType: commandType);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Automated methods for: Insert, Update, Delete

        /// These methods are provided for your convenience.
        /// For simple objects they will work fine, 
        /// but please be aware that they will not cover more complex scenarios!
        /// Id column is assumed to be of type int IDENTITY.
        /// Reflection is used to create appropriate SQL statements.
        /// Even if reflection is costly in itself, the average gain 
        /// compared to Entity Framework is approximately a factor 10!
        /// Key property is determined by convention 
        /// (Id, TypeNameId or TypeName_Id) or by custom attribute [DapperKey].
        /// All properties with public setters are included. 
        /// Exclusion can be manually made with custom attribute [DapperIgnore].
        /// If key property is mapped to single database Identity column, 
        /// then it is automatically reflected back to object.

        protected T Insert<T>(T obj)
        {
            T result;
            var propertyContainer = ParseProperties(obj);
            var sql = string.Format("DECLARE @InsertedRows AS TABLE (Id int); " +
                                    "INSERT INTO [{0}] ({1}) OUTPUT INSERTED.Id INTO @InsertedRows VALUES(@{2});" +
                                    "SELECT id FROM @InsertedRows;",
                            typeof(T).Name,
                            string.Join(", ", propertyContainer.ValueNames),
                            string.Join(", @", propertyContainer.ValueNames));

            try
            {
                using (var connection = CreateConnection())
                {
                    result = connection.QuerySingle<T>(sql, propertyContainer.ValuePairs, commandType: CommandType.Text);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected int Update<T>(T obj)
        {
            var propertyContainer = ParseProperties(obj);
            var sqlIdPairs = GetSqlPairs(propertyContainer.IdNames);
            var sqlValuePairs = GetSqlPairs(propertyContainer.ValueNames);
            var sql = string.Format("UPDATE [{0}] SET {1} WHERE {2}", typeof(T).Name, sqlValuePairs, sqlIdPairs);

            try
            {
                return Execute(CommandType.Text, sql, propertyContainer.AllPairs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected int Delete<T>(T obj)
        {
            var propertyContainer = ParseProperties(obj);
            var sqlIdPairs = GetSqlPairs(propertyContainer.IdNames);
            var sql = string.Format("DELETE FROM [{0}] WHERE {1}", typeof(T).Name, sqlIdPairs);

            try
            {
                return Execute(CommandType.Text, sql, propertyContainer.IdPairs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected int Execute(CommandType commandType, string sql, object parameters = null)
        {
            using (var connection = CreateConnection())
            {
                return connection.Execute(sql, parameters, commandType: commandType);
            }
        }

        #endregion

        #region Reflection support

        /// <summary>
        /// Retrieves a Dictionary with name and value 
        /// for all object properties matching the given criteria.
        /// </summary>
        private static PropertyContainer ParseProperties<T>(T obj)
        {
            var propertyContainer = new PropertyContainer();

            var typeName = typeof(T).Name;
            var validKeyNames = new[] { "Id", string.Format("{0}Id", typeName), string.Format("{0}_Id", typeName) };

            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                // Skip reference types (but still include string!)
                if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
                    continue;

                // Skip methods without a public setter
                if (property.GetSetMethod() == null)
                    continue;

                // Skip methods specifically ignored
                if (property.IsDefined(typeof(DapperIgnore), false))
                    continue;

                var name = property.Name;
                var value = typeof(T).GetProperty(property.Name).GetValue(obj, null);

                if (property.IsDefined(typeof(DapperKey), false) || validKeyNames.Contains(name))
                {
                    propertyContainer.AddId(name, value);
                }
                else
                {
                    if (value != null)
                        propertyContainer.AddValue(name, value);
                }
            }

            return propertyContainer;
        }

        /// <summary>
        /// Create a commaseparated list of value pairs on 
        /// the form: "key1=@value1, key2=@value2, ..."
        /// </summary>
        private static string GetSqlPairs(IEnumerable<string> keys, string separator = ", ")
        {
            var pairs = keys.Select(key => string.Format("{0}=@{0}", key)).ToList();
            return string.Join(separator, pairs);
        }

        private void SetId<T>(T obj, int id, IDictionary<string, object> propertyPairs)
        {
            if (propertyPairs.Count == 1)
            {
                var propertyName = propertyPairs.Keys.First();
                var propertyInfo = obj.GetType().GetProperty(propertyName);
                if (propertyInfo.PropertyType == typeof(int))
                {
                    propertyInfo.SetValue(obj, id, null);
                }
            }
        }

        #endregion

        private class PropertyContainer
        {
            private readonly Dictionary<string, object> _ids;
            private readonly Dictionary<string, object> _values;

            #region Properties

            internal IEnumerable<string> IdNames
            {
                get { return _ids.Keys; }
            }

            internal IEnumerable<string> ValueNames
            {
                get { return _values.Keys; }
            }

            internal IEnumerable<string> AllNames
            {
                get { return _ids.Keys.Union(_values.Keys); }
            }

            internal IDictionary<string, object> IdPairs
            {
                get { return _ids; }
            }

            internal IDictionary<string, object> ValuePairs
            {
                get { return _values; }
            }

            internal IEnumerable<KeyValuePair<string, object>> AllPairs
            {
                get { return _ids.Concat(_values); }
            }

            #endregion

            #region Constructor

            internal PropertyContainer()
            {
                _ids = new Dictionary<string, object>();
                _values = new Dictionary<string, object>();
            }

            #endregion

            #region Methods

            internal void AddId(string name, object value)
            {
                _ids.Add(name, value);
            }

            internal void AddValue(string name, object value)
            {
                _values.Add(name, value);
            }

            #endregion
        }
    }
}
