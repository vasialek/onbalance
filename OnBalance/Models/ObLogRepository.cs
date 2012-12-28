using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq;
using System.Configuration;

namespace OnBalance.Models
{
    public class ObLogRepository
    {

        protected DataContext _db = new DataContext(ConfigurationManager.ConnectionStrings["OnlineBalanceConnectionString"].ToString());

        public IQueryable<ObLog> Logs
        {
            get
            {
                return _db.GetTable<ObLog>();
            }
        }

        public IList<ObLog> GetLast(int offset, int limit)
        {
            return Logs.OrderByDescending(x => x.Date)
                .Skip(offset)
                .Take(limit)
                .ToList();
        }

    }
}