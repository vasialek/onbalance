using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using OnBalance.Domain.Entities;

namespace OnBalance.ViewModels.Organizations
{
    public class OrganizationEditViewModel
    {
        protected IList<Organization> _children = null;
        protected IList<MembershipUser> _users = null;

        public Organization Organization { get; set; }

        public Organization Parent { get; set; }

        public IList<Organization> Children
        {
            get
            {
                if(_children == null)
                {
                    _children = new List<Organization>();
                }
                return _children;
            }
            set
            {
                _children = value;
            }
        }

        public IList<MembershipUser> Users
        {
            get
            {
                if(_users == null)
                {
                    _users = new List<MembershipUser>();
                }
                return _users;
            }
            set
            {
                _users = value;
            }
        }
    }
}
