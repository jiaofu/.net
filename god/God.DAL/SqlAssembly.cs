using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace God.DAL
{
    public class SqlAssembly
    {

        public static IDbConnection GetConnection(string connectionString)
        {
            IDbConnection conn = null;
            switch (AdoHelper.DbType)
            {
                case DatabaseType.SQLSERVER:
                    conn = new SqlConnection(connectionString);
                    break;
                case DatabaseType.MYSQL:
                    conn = new MySql.Data.MySqlClient.MySqlConnection(connectionString);
                    break;
                case DatabaseType.ACCESS:
                    conn = new OleDbConnection(connectionString);
                    break;
                default:
                    throw new Exception("数据库类型目前不支持！");
            }

            return conn;

        }


        public static IDbCommand GetCommand()
        {
            IDbCommand comm = null;
            switch (AdoHelper.DbType)
            {
                case DatabaseType.SQLSERVER:
                    comm = new SqlCommand();
                    break;
                case DatabaseType.MYSQL:
                    comm = new MySql.Data.MySqlClient.MySqlCommand();
                    break;
                case DatabaseType.ACCESS:
                    comm = new OleDbCommand();
                    break;
                default:
                    throw new Exception("数据库类型目前不支持！");
            }
            return comm;

        }
        /// <summary>  
        /// 根据配置文件中所配置的数据库类型  
        /// 来获取命令参数中的参数符号oracle为":",sqlserver为"@"  
        /// </summary>  
        /// <returns></returns>  
        public static string CreateDbParmCharacter()
        {
            string character = string.Empty;

            switch (AdoHelper.DbType)
            {
                case DatabaseType.SQLSERVER:
                    character = "@";
                    break;
                case DatabaseType.MYSQL:
                    character = "@";
                    break;
                case DatabaseType.ORACLE:
                    character = ":";
                    break;
                case DatabaseType.ACCESS:
                    character = "@";
                    break;
                default:
                    throw new Exception("数据库类型目前不支持！");
            }

            return character;
        }
        /// <summary>  
        /// 根据配置文件中所配置的数据库类型  
        /// 和传入的参数来创建相应数据库的参数数组对象  
        /// </summary>  
        /// <returns></returns>  
        public static IDbDataParameter[] CreateDbParameters(int size)
        {
            int i = 0;
            IDbDataParameter[] param = null;
            switch (AdoHelper.DbType)
            {
                case DatabaseType.SQLSERVER:
                    param = new SqlParameter[size];
                    while (i < size) { param[i] = new SqlParameter(); i++; }
                    break;
                case DatabaseType.MYSQL:
                    param = new MySql.Data.MySqlClient.MySqlParameter[size];
                    while (i < size) { param[i] = new MySqlParameter(); i++; }
                    break;
                case DatabaseType.ACCESS:
                    param = new OleDbParameter[size];
                    while (i < size) { param[i] = new OleDbParameter(); i++; }
                    break;
                default:
                    throw new Exception("数据库类型目前不支持！");  
            }
            return param;
        }

    }
}