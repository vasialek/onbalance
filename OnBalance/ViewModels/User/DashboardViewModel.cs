using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using OnBalance.Domain.Entities;
using OnBalance.Domain.Concrete;

namespace OnBalance.ViewModels.User
{
    public class DashboardViewModel
    {

        public List<Organization> Shops { get; set; }

        public List<OnBalance.Models.Task> Imports { get; set; }

        public List<OnBalance.Models.Task> Exports { get; set; }

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
            Imports = new List<OnBalance.Models.Task>();
            Exports = new List<OnBalance.Models.Task>();
            LastPos = new List<Organization>();
            LastUsers = new List<MembershipUser>();
        }


        public void Init()
        {
            //Shops = new List<Organization>();
            Shops = new EfOrganizationRepository().GetByParentId(500013).ToList();
        }
    }
}