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