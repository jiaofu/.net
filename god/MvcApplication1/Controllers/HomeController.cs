using God.DAL;
using MvcApplication1.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication1.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            string sql = "SELECT * FROM AC_Role ";
            MySqlHelper.connstr = AdoHelper.ConnectionString;
          var list=  MySqlHelper.GetList<Class1>(sql);
            var sql1="select * from  AC_Role limit 10";
                var sql2="select count(1) from AC_Role";
                int count = 0;
                //var list2 = MySqlHelper.GetList<Class1>(sql1, sql2,out count);
                var sqlinsert = "insert into AC_Role(RoleName,IsAdmin) value('ceshib',0);insertc into AC_Role(RoleName,IsAdmin) value('ceshib',0)";
                MySqlHelper.ExecuteNonQuery(sqlinsert, null, CommandType.Text, true);

            return View();
        }

    }
}
