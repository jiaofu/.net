using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace God.DAL
{
  public  class CommonUtils
    {

        // <summary>
        // 根据传入的Key获取配置文件中的Value值
        // </summary>
        // <param name="Key"></param>
        // <returns></returns>
        public static string GetConfigValueByKey(string Key)
        {
            try
            {
                return System.Configuration.ConfigurationManager.AppSettings[Key].ToString();
            }
            catch
            {
                throw new Exception("web.config中 Key=\"" + Key + "\"未配置或配置错误！");
            }
        }
      /// <summary>
      /// 获取连接字符串
      /// </summary>
      /// <param name="key"></param>
      /// <returns></returns>
        public static string GetConnectionString(string key)
        {
            try
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings[key].ToString();
            }
            catch
            {
                throw new Exception("web.config中ConnectionStrings name=\"" + key + "\"未配置或配置错误！");
            }
        }

        // <summary>
        // 用于字符串和枚举类型的转换
        // </summary>
        // <typeparam name="T"></typeparam>
        // <param name="value"></param>
        // <returns></returns>
        public static T EnumParse<T>(string value)
        {
            try
            {
                return (T)Enum.Parse(typeof(T), value);
            }
            catch
            {
                throw new Exception("传入的值与枚举值不匹配。");
            }
        }
      
    
       
    }
}
