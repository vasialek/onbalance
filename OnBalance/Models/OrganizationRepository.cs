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
        /// <summary>
        /// Property to work with DB
        /// </summary>
        protected DataContext _db = null;

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
                return _db.GetTable<Organization>()
                    .Where(x => x.StatusId != 0);
            }
        }

        public void Save(Organization model)
        {
            var db = _db.GetTable<Organization>();
            db.InsertOnSubmit(model);
            db.Context.SubmitChanges();
        }

    }
}