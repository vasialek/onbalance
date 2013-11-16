using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OnBalance.Domain.Entities;
using System.Web.Security;

namespace OnBalance.Domain.Abstract
{
    public interface IOrganizationRepository
    {
        IQueryable<Organization> Organizations { get; }

        IQueryable<Organization> Companies { get; }

        Organization GetById(int p);

        void Save(Organization model);

        IList<Organization> GetByParentId(int p);

        IList<MembershipUser> GetUsersInOrganization(int p);

        void SubmitChanges();

        IList<Organization> GetByParentId(int p, bool p_2);
    }
}
