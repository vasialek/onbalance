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
        /// Adds Organization to DB and submit changes
        /// </summary>
        /// <param name="model"></param>
        public void Save(Organization model)
        {
            var db = _dataContext.GetTable<Organization>();
            db.InsertOnSubmit(model);
            db.Context.SubmitChanges();
        }


        public void SubmitChanges()
        {
            _dataContext.SubmitChanges();
        }
    }
}
