using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace God.DAL
{
    public class MySqlHelper
    {
        public static string connstr{get;set;}
      
        private static IDbConnection connection = null;
        private static IDbTransaction iDbTransaction { get; set; }
        public static int ExecuteNonQuery(string queryStr, Dictionary<string, object> dic = null, CommandType commandType = CommandType.Text, bool Transaction = false)
        {
            try
            {
                using (connection = SqlAssembly.GetConnection(connstr))
                {

                    using (IDbCommand command = connection.CreateCommand())
                    {
                        try
                        {
                            PrepareCommand(queryStr, connection, command, dic, commandType, Transaction);

                            int count = command.ExecuteNonQuery();
                            if (Transaction)
                            {
                                command.Transaction.Commit();
                                return count;
                            }
                            return count;
                        }
                        catch (Exception ex)
                        {
                            if (Transaction)
                            {
                                iDbTransaction.Rollback();
                            }
                            throw new Exception(ex.ToString());
                        }
                 
                    }
                }
            }
            catch (Exception ex)
            {
     
                throw new Exception(ex.ToString());
            }
        
        }
        public static Task<int> ExecuteNonQueryAsync(string queryStr, Dictionary<string, object> dic = null, CommandType commandType = CommandType.Text, bool Transaction = false)
        {
            try
            {
                using (connection = SqlAssembly.GetConnection(connstr))
                {

                    using (IDbCommand command = connection.CreateCommand())
                    {
                        try
                        {
                            PrepareCommand(queryStr, connection, command, dic, commandType, Transaction);
                            var count = Task.Run(() => command.ExecuteNonQuery());
                            if (Transaction)
                            {
                                command.Transaction.Commit();
                                return count;
                            }
                            return count;
                        }
                        catch (Exception ex)
                        {
                            if (Transaction)
                            {
                                iDbTransaction.Rollback();
                            }
                            throw new Exception(ex.ToString());
                        }
   
                    }
                }
            }
            catch (Exception ex)
            {
        
                throw new Exception(ex.ToString());
            }
    
        }
        public static object ExecuteScalar(string queryStr, Dictionary<string, object> dic = null, CommandType commandType = CommandType.Text)
        {
            try
            {
                using (connection = SqlAssembly.GetConnection(connstr))
                {

                    using (IDbCommand command = connection.CreateCommand())
                    {
                        PrepareCommand(queryStr, connection, command, dic, commandType);
                        return command.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        public static Task<object> ExecuteScalarAsync(string queryStr, Dictionary<string, object> dic, CommandType commandType = CommandType.Text)
        {
            try
            {
                using (connection = SqlAssembly.GetConnection(connstr))
                {

                    using (IDbCommand command = connection.CreateCommand())
                    {
                        PrepareCommand(queryStr, connection, command, dic, commandType);
                        return Task.Run(() => command.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
     
        }
        public static IList<T> GetList<T>(string queryStr, Dictionary<string, object> dic=null, CommandType commandType = CommandType.Text) where T : class,new()
        {
            IDataReader dataReader = null;
            try
            {
                using (connection = SqlAssembly.GetConnection(connstr))
                {
                    using (IDbCommand command = connection.CreateCommand())
                    {
                        PrepareCommand(queryStr, connection, command, dic, commandType);
                        dataReader = command.ExecuteReader();
                        return ReadConvertList<T>(dataReader);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                if (dataReader != null && !dataReader.IsClosed)
                {
                    dataReader.Close();
                }
                connection.Close(); ;
            }
        }
        /// <summary>
        /// 分页同步
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryStrList"></param>
        /// <param name="queryStrCount"></param>
        /// <param name="count"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static IList<T> GetList<T>(string queryStrList, string queryStrCount, out int count, Dictionary<string, object> dic=null) where T : class,new()
        {
            IDataReader dataReader = null;
            count = 0;
            try
            {
                using (connection = SqlAssembly.GetConnection(connstr))
                {
                    using (IDbCommand command = connection.CreateCommand())
                    {
                        PrepareCommand(queryStrCount, connection, command, dic, CommandType.Text);
                        int.TryParse(command.ExecuteScalar().ToString(), out count);
                        PrepareCommand(queryStrList, connection, command, dic, CommandType.Text);
                        dataReader = command.ExecuteReader();

                        return ReadConvertList<T>(dataReader);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                if (dataReader != null && !dataReader.IsClosed)
                {
                    dataReader.Close();
                }
                connection.Close(); ;
            }

        }
        /// <summary>
        /// 分页异步
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryStrList"></param>
        /// <param name="queryStrCount"></param>
        /// <param name="count"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static Task<IList<T>> GetListAsync<T>(string queryStrList, string queryStrCount, out int count, Dictionary<string, object> dic = null) where T : class,new()
        {
            IDataReader dataReader = null;
            count = 0;
            try
            {
                using (connection = SqlAssembly.GetConnection(connstr))
                {
                    using (IDbCommand command = connection.CreateCommand())
                    {
                        PrepareCommand(queryStrList, connection, command, dic, CommandType.Text);
                        int.TryParse(command.ExecuteScalar().ToString(), out count);
                        PrepareCommand(queryStrList, connection, command, dic, CommandType.Text);
                        var taskdataReder = Task.Run(() => command.ExecuteReader());


                        return ReadConvertListAsync<T>(taskdataReder);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                if (dataReader != null && !dataReader.IsClosed)
                {
                    dataReader.Close();
                }
                connection.Close(); ;
            }

        }
        /// <summary>
        /// 同步绑定数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static IList<T> ReadConvertList<T>(IDataReader reader) where T : new()
        {

            using (reader)
            {
                var columnName = new List<string>(reader.FieldCount);
                for (int i = 0; i < reader.FieldCount; i++)
                {

                    columnName.Add(reader.GetName(i).ToLower());

                }
                IList<T> list = new List<T>();
                while (reader.Read())
                {
                    T t = new T(); ;
                    var thisType= t.GetType();
                   
                    var propertys =thisType.GetProperties( BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance).Where(q => columnName.Contains(q.Name.ToLower())).Where(q => !(reader[q.Name] is DBNull)).ToList();
                    propertys.ForEach(q => q.SetValue(t, reader[q.Name]));
                    var Fields = thisType.GetFields().Where(q => columnName.Contains(q.Name.ToLower())).Where(q => !(reader[q.Name] is DBNull)).ToList();
                    Fields.ForEach(q => q.SetValue(t, reader[q.Name]));
                    list.Add(t);
                }
                return list;
            }
        }
        /// <summary>
        /// 异步绑定数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="treader"></param>
        /// <returns></returns>
        private async static Task<IList<T>> ReadConvertListAsync<T>(Task<IDataReader> treader) where T : new()
        {
            using (treader)
            {
                IDataReader reader = await treader;
                var columnName = new List<string>(reader.FieldCount);
                for (int i = 0; i < reader.FieldCount; i++)
                {

                    columnName.Add(reader.GetName(i).ToLower());

                }
                IList<T> list = new List<T>();
                while (reader.Read())
                {
                    T t = new T(); ;
                    var thisType = t.GetType();
                    var propertys = thisType.GetProperties(BindingFlags.Public).Where(q => columnName.Contains(q.Name.ToLower())).Where(q => !(reader[q.Name] is DBNull)).ToList();
                    propertys.ForEach(q => q.SetValue(t, reader[q.Name]));
                    var Fields = thisType.GetFields().Where(q => columnName.Contains(q.Name.ToLower())).Where(q => !(reader[q.Name] is DBNull)).ToList();
                    Fields.ForEach(q => q.SetValue(t, reader[q.Name]));
                    list.Add(t);
                }
                return list;
            }
        }
        /// <summary>
        /// 判断是否为空
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        private static bool IsNullOrDBNull(object obj)
        {
            return ((obj is DBNull) || string.IsNullOrEmpty(obj.ToString()));
         
        }
        private static void PrepareCommand(string sql, IDbConnection connection, IDbCommand command, Dictionary<string, object> dic = null, CommandType commandType = CommandType.Text, bool isTransaction = false)
        {
            command.CommandText = sql;
            command.Connection = connection;
            
            if (dic != null)
            {
                var cmdparams = SqlAssembly.CreateDbParameters(dic.Count);

                int i=0;
                foreach(var v in dic)
                {
                 cmdparams[i].ParameterName =  v.Key;
                 cmdparams[i].Value=v.Value;
                 command.Parameters.Add(v);
                }
        
            }
            if (connection.State != ConnectionState.Open)
                connection.Open();
            if (isTransaction)
            {
                iDbTransaction = connection.BeginTransaction();
                command.Transaction = iDbTransaction;
            }

        }

     
    }
}
