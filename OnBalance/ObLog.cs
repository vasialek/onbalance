using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using log4net.Config;

namespace OnBalance
{
    public static class ObLog
    {

        private static ILog _log = null;

        public static ILog Log
        {
            get
            {
                if( _log == null )
                {
                    _log = LogManager.GetLogger("OnlineBalance");
                    XmlConfigurator.Configure();
                }

                return _log;
            }
        }

    }
}