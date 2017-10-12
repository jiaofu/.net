using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1
{
    //就是继承了这个IAsyncResult接口,所以就可以是回调的参数类
    public class MyAsyncResult : IAsyncResult
    {
        //这个接口的实现
        public object AsyncState { get; private set; }
        public System.Threading.WaitHandle AsyncWaitHandle { get; private set; }
        public bool CompletedSynchronously { get { return false; } }
        public bool IsCompleted { get; private set; }
        //一些个参数
        public HttpContext Context { get; set; }
        public AsyncCallback CallBack { get; set; }
        public string SessionId { get; set; }
        public string Message { get; set; }
        //构造函数
        public MyAsyncResult(HttpContext context, AsyncCallback cb, string sessionId)
        {
            this.SessionId = sessionId;
            this.Context = context;
            this.CallBack = cb;
        }
        //这个方法对于的是MyHandler调用哪个方法,
        //它的主要作用是,用某种浏览器检测工具,也就是能检测所有请求的工具,就能看出,它是结束当前的请求,在用js马上开始另一个请求,好处就是,感觉这个客服端是长连接的
        public void SetCompleted(bool iscompleted)
        {
            this.IsCompleted = iscompleted;
            if (iscompleted && this.CallBack != null)
            {
                CallBack(this);
            }
        }
    }
}