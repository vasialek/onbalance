using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq;
using System.Configuration;

namespace OnBalance.Models
{
    public class OrganizationRepository : BaseRepository
    {
        //public OrganizationRepository()
        //    : base(ConfigurationManager.ConnectionStrings["OnlineBalanceConnectionString"].ToString())
        //{

        //}

        //public OrganizationRepository(string connectionString)
        //    : base(connectionString)
        //{

        //}

        public IQueryable<Organization> Organizations
        {
            get
            {
                return _dataContext.GetTable<Organization>();
            }
        }

        public void Save(Organization model)
        {
            var db = _dataContext.GetTable<Organization>();
            db.InsertOnSubmit(model);
            db.Context.SubmitChanges();
        }

    }
}