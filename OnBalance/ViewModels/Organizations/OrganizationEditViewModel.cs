using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnBalance.Models;
using System.Web.Security;

namespace OnBalance.ViewModels.Organizations
{
    public class OrganizationEditViewModel
    {
        public Organization Organization { get; set; }

        public Organization Parent { get; set; }

        public IList<Organization> Children { get; set; }

        public IList<MembershipUser> Users { get; set; }
    }
}
