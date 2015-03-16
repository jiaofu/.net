using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace God.DAL
{
  public  class AdoHelper
    {
        //获取数据库类型
        private static string strDbType = CommonUtils.GetConfigValueByKey("dbType").ToUpper();

        //将数据库类型转换成枚举类型
        public static DatabaseType DbType = CommonUtils.EnumParse<DatabaseType>(strDbType);

        //获取数据库连接字符串
        public static string ConnectionString = CommonUtils.GetConnectionString("connectionString");

        //获取数据库命名参数符号，比如@(SQLSERVER)、:(ORACLE)
        public static string DbParmChar = SqlAssembly.CreateDbParmCharacter();
    }
}
