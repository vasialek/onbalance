using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace OnBalance.Core
{
    public class MySettings
    {

        public static string ConnectionStringDefault
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["OnlineBalanceConnectionString"].ConnectionString;
            }
        }

    }
}
