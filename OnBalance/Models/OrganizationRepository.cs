using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq;
using System.Configuration;
using System.Web.Security;

namespace OnBalance.Models
{
    public class OrganizationRepository : BaseRepository
    {

        /// <summary>
        /// Gets list of all items
        /// </summary>
        public IQueryable<Organization> Items
        {
            get
            {
                return _dataContext.GetTable<Organization>();
            }
        }

        /// <summary>
        /// Gets list of companies (parent_id == 0)
        /// </summary>
        public IQueryable<Organization> Companies
        {
            get
            {
                return Items.Where(x => x.ParentId == 0);
            }
        }

        /// <summary>
        /// Returns Organization by ID or null if not found
        /// </summary>
        /// <param name="id"></param>
        public Organization GetById(int id)
        {
            return Items.SingleOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// Returns list of ALL children by specified parent ID (parent is not included)
        /// </summary>
        /// <param name="parentId"></param>
        public IList<Organization> GetByParentId(int parentId)
        {
            return Items.Where(x => x.ParentId == parentId).ToList();
        }

        /// <summary>
        /// Adds Organization to DB and submit changes
        /// </summary>
        /// <param name="model"></param>
        public void Save(Organization model)
        {
            var db = _dataContext.GetTable<Organization>();
            if(model.Id < 1)
            {
                db.InsertOnSubmit(model);
            } else
            {
                //Organization record = db.SingleOrDefault(x => x.Id == model.Id);
                db.Attach(model);
                //record = model;
            }
            db.Context.SubmitChanges();
        }


        public void SubmitChanges()
        {
            _dataContext.SubmitChanges();
        }


        public IList<MembershipUser> GetUsersInOrganization(int organizationId)
        {
            using(DataContext dataContext = new DataContext(ConfigurationManager.ConnectionStrings["OnlineBalanceConnectionString"].ToString()))
            {
                var q = dataContext.GetTable<UserOrganization>()
                    .Where(x => x.OrganizationId == organizationId)
                    .Select(x => Membership.GetUser(x.Username));
                return q.ToList();
	        }
        }

    }
}
