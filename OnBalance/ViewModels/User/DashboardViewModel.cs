using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnBalance.Models;
using System.Web.Security;

namespace OnBalance.ViewModels.User
{
    public class DashboardViewModel
    {

        public List<Organization> Shops { get; set; }

        public List<Task> Imports { get; set; }

        public List<Task> Exports { get; set; }

        /// <summary>
        /// Gets list of last created/registered users
        /// </summary>
        public IList<MembershipUser> LastUsers { get; set; }

        /// <summary>
        /// Gets list of last created POS
        /// </summary>
        public IList<Organization> LastPos { get; set; }

        public DashboardViewModel()
        {
            Shops = new List<Organization>();
            Imports = new List<Task>();
            Exports = new List<Task>();
            LastPos = new List<Organization>();
            LastUsers = new List<MembershipUser>();
        }


        public void Init()
        {
            var dbOrg = new OrganizationRepository();
            Shops = dbOrg.Companies.ToList();
        }
    }
}