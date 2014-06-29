using log4net;
using OnBalance.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnBalance.ParsersUnitTest
{
    public class ObLog4NetLogger : IObLogger
    {

        private string _nameOfLogger = "ObLog4NetLogger";

        private ILog _logger = null;
        private ILog Logger
        {
            get
            {
                if(_logger == null)
                {
                    _logger = LogManager.GetLogger(_nameOfLogger);
                    log4net.Config.XmlConfigurator.Configure();
                }
                return _logger;
            }
        }

        public ObLog4NetLogger(string loggerName)
        {
            _nameOfLogger = loggerName;
        }

        public void Debug(string msg)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug(msg);
            }
        }

        public void DebugFormat(string fmt, params object[] args)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.DebugFormat(fmt, args);
            }
        }

        public void Info(string msg)
        {
            if (Logger.IsInfoEnabled)
            {
                Logger.Info(msg);
            }
        }

        public void InfoFormat(string fmt, params object[] args)
        {
            if (Logger.IsInfoEnabled)
            {
                Logger.InfoFormat(fmt, args);
            }
        }

        public void Warn(string msg)
        {
            if (Logger.IsWarnEnabled)
            {
                Logger.Warn(msg);
            }
        }

        public void WarnFormat(string fmt, params object[] args)
        {
            if (Logger.IsWarnEnabled)
            {
                Logger.WarnFormat(fmt, args);
            }
        }

        public void Error(string msg)
        {
            if (Logger.IsErrorEnabled)
            {
                Logger.Error(msg);
            }
        }

        public void Error(string msg, Exception ex)
        {
            if (Logger.IsErrorEnabled)
            {
                Logger.Error(msg, ex);
            }
        }

        public void ErrorFormat(string fmt, params object[] args)
        {
            if (Logger.IsErrorEnabled)
            {
                Logger.ErrorFormat(fmt, args);
            }
        }
    }
}
