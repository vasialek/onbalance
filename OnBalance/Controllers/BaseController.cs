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

        protected bool _isDebugEnabled = false;
        protected bool _isInfoEnabled = false;
        protected bool _isWarnEnabled = false;
        protected bool _isErrorEnabled = false;

        public string Layout { get { return "_Layout"; } }

        //public class Log
        //{

        //    public static void InfoFormat(string p, params object[] args)
        //    {
        //        //throw new NotImplementedException();
        //    }
        //    public static void DebugFormat(string p, params object[] args)
        //    {
        //        //throw new NotImplementedException();
        //    }

        //    internal static void Error(string p, Exception ex)
        //    {
        //        //throw new NotImplementedException();
        //    }
        //}

        ILog _log = null;
        public ILog Logger
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
            _isDebugEnabled = Logger.IsDebugEnabled;
            _isInfoEnabled = Logger.IsInfoEnabled;
            _isWarnEnabled = Logger.IsWarnEnabled;
            _isErrorEnabled = Logger.IsErrorEnabled;
        }

        #region " Logger methods "

        protected void Debug(object msg)
        {
            if(_isInfoEnabled)
            {
                Logger.Debug(msg);
            }
        }

        protected void DebugFormat(string fmt, params object[] args)
        {
            if(_isDebugEnabled)
            {
                Logger.DebugFormat(fmt, args);
            }
        }

        protected void Info(object msg)
        {
            if(_isInfoEnabled)
            {
                Logger.Info(msg);
            }
        }

        protected void InfoFormat(string fmt, params object[] args)
        {
            if(_isInfoEnabled)
            {
                Logger.InfoFormat(fmt, args);
            }
        }

        protected void Warn(object msg)
        {
            if(_isWarnEnabled)
            {
                Logger.Warn(msg);
            }
        }

        protected void WarnFormat(string fmt, params object[] args)
        {
            if(_isWarnEnabled)
            {
                Logger.WarnFormat(fmt, args);
            }
        }

        protected void Error(object msg)
        {
            if(_isErrorEnabled)
            {
                Logger.Error(msg);
            }
        }

        protected void Error(object msg, Exception ex)
        {
            if(_isErrorEnabled)
            {
                Logger.Error(msg, ex);
            }
        }

        protected void ErrorFormat(string fmt, params object[] args)
        {
            if(_isErrorEnabled)
            {
                Logger.ErrorFormat(fmt, args);
            }
        }

        #endregion


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
