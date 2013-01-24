using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq;
using System.Configuration;

namespace OnBalance.Models
{
    public class BaseRepository/* : DataContext */
    {
        /// <summary>
        /// Property to work with DB
        /// </summary>
        protected DataContext _dataContext = null;

        public BaseRepository()
        {
            _dataContext = new DataContext(ConfigurationManager.ConnectionStrings["OnlineBalanceConnectionString"].ToString());
        }

    }
}