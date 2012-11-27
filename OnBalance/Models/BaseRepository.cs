using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net.Config;
using log4net;
using System.Data.Linq;
using System.Configuration;

namespace OnBalance.Models
{
    public class BaseRepository : DataContext
    {

        protected ILog _log = null;
        public ILog Log
        {
            get
            {
                if(_log == null)
                {
                    _log = LogManager.GetLogger("OnBalance");
                    XmlConfigurator.Configure();
                }

                return _log;
            }
        }

        public BaseRepository()
            : base(ConfigurationManager.ConnectionStrings["OnlineBalanceConnectionString"].ToString())
        {

        }

    }
}