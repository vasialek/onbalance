using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using log4net;
using log4net.Config;

namespace OnBalance.Controllers
{
    public class BaseController : Controller
    {

        public string Layout { get { return "_Layout"; } }

        public class Log
        {

            public static void InfoFormat(string p, params object[] args)
            {
                //throw new NotImplementedException();
            }
            public static void DebugFormat(string p, params object[] args)
            {
                //throw new NotImplementedException();
            }

            internal static void Error(string p, Exception ex)
            {
                //throw new NotImplementedException();
            }
        }

        public BaseController()
        {
            ILog log = LogManager.GetLogger("OnBalance");
            XmlConfigurator.Configure();
            log.InfoFormat("Starting...");
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            StringBuilder sb = new StringBuilder();
            foreach(var name in Request.Form.AllKeys)
            {
                sb.AppendFormat("{0}: {1}", name, Request[name]).AppendLine();
            }

            //byte[] ba = Encoding.ASCII.GetBytes(sb.ToString());
            //filterContext.HttpContext.Response.OutputStream.Write(ba, 0, ba.Length);
        }

    }
}
