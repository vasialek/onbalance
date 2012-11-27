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

        protected ILog _log = null;
        public ILog Log
        {
            get
            {
                if( _log == null )
                {
                    _log = LogManager.GetLogger("OnBalance");
                    XmlConfigurator.Configure();
                }

                return _log;
            }
        }

        public BaseController()
        {
            Log.Info("Working...");
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
