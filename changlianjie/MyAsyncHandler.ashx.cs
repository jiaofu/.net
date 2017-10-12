using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1
{
    public class MyAsyncHandler : IHttpAsyncHandler
    {
        //这个集合 用于存放 所有请求的

        public static List<MyAsyncResult> Queue = new List<MyAsyncResult>();
        public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
        {
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            string sessionId = context.Request.QueryString["sessionId"];
            //查找Queue这个集合 SessionId ==传过来的sessionId !=null
            if (Queue.Find(q => q.SessionId == sessionId) != null)
            {
                int index = Queue.IndexOf(Queue.Find(q => q.SessionId == sessionId));
                //把HttpContext对象的实例等于当前请求的所有信息
                Queue[index].Context = context;
                Queue[index].CallBack = cb;
                return Queue[index];
            }
            //MyAsyncResult 这个类是 回调的参数类(相当于 你定义一个事件 使用的泛型的 public event EventHandler Events; MyEvargs这个类继承了EventArgs 同样的道理)
            MyAsyncResult asyncResult = new MyAsyncResult(context, cb, sessionId);
            Queue.Add(asyncResult);
            return asyncResult;
        }

        public void EndProcessRequest(IAsyncResult result)
        {
            MyAsyncResult rslt = (MyAsyncResult)result;
            //向别的客服端推送 某个 客服端发送的 信息
            rslt.Context.Response.Write(rslt.Message);
            rslt.Message = string.Empty;
        }
        //为什么不实现这个方法 (因为IhttpAsyncHandler接口继承了IHttpHandler这个接口,所以实现接口的时候,就实现了它,但是 我们不管它)
        #region IHttpHandler 成员 不实现
        public bool IsReusable
        {
            get { return true; }
        }
        public void ProcessRequest(HttpContext context)
        {
        }
        #endregion
    }
}