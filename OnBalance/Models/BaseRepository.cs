using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq;
using System.Configuration;

namespace OnBalance.Models
{
    public class BaseRepository : DataContext
    {

        public BaseRepository()
            : base(ConfigurationManager.ConnectionStrings["OnlineBalanceConnectionString"].ToString())
        {

        }

    }
}