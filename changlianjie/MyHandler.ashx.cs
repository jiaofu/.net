using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1
{
    public class MyHandler : IHttpHandler
    {
        //这个属性,和方法 都是实现 IHttpHandler 的
        public bool IsReusable
        {
            get { return true; }
        }
        public void ProcessRequest(HttpContext context)
        {
            //设置不让客服端缓存
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            List<MyAsyncResult> userlist = MyAsyncHandler.Queue;
            string sessionId = context.Request.QueryString["sessionId"];
            //总人数
            string i = userlist.Count.ToString();
            foreach (MyAsyncResult res in userlist)
            {

                //如果不是自己就推
                if (res.SessionId != sessionId)
                {
                    //激发callback，结束请求
                    res.Message = i;
                    res.SetCompleted(true);
                }
            }
        }
    }
}