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

        Organization GetById(int organizationId);

        IList<Organization> GetByParentId(int parentOrganizationId);

        IList<Organization> GetByParentId(int parentOrganizationId, bool includeParent);

        IList<MembershipUser> GetUsersInOrganization(int organizationId);

        void Save(Organization model);

        void SubmitChanges();
    }
}
