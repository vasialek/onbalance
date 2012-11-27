using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq;
using System.Configuration;

namespace OnBalance.Models
{
    public class OrganizationRepository : DataContext
    {

        public OrganizationRepository()
            : base(ConfigurationManager.ConnectionStrings["OnlineBalanceConnectionString"].ToString())
        {

        }

        public OrganizationRepository(string connectionString)
            : base(connectionString)
        {

        }

        public IQueryable<Organization> Organizations
        {
            get
            {
                var db = this.GetTable<Organization>();
                return db.Where(x => x.StatusId != 0);
            }
        }

        public void Save(Organization model)
        {
            var db = GetTable<Organization>();
            db.InsertOnSubmit(model);
            db.Context.SubmitChanges();
        }

    }
}