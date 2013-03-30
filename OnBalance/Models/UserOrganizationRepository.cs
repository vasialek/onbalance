using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnBalance.Models
{
    public class UserOrganizationRepository : BaseRepository
    {
        /// <summary>
        /// Gets all items in table `user_organization`
        /// </summary>
        public IQueryable<UserOrganization> Items { get { return _dataContext.GetTable<UserOrganization>(); } }

        public void AddUserToOrganization(string username, int orgId)
        {
            var db = _dataContext.GetTable<UserOrganization>();
            db.InsertOnSubmit(new UserOrganization
            {
                Username = username,
                OrganizationId = orgId
            });
            db.Context.SubmitChanges();
        }
    }
}