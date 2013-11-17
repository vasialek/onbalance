using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OnBalance.Domain.Abstract;
using OnBalance.Domain.Entities;
using System.Web.Security;
using OnBalance.Domain.Primitives;

namespace OnBalance.Domain.Concrete
{
    public class EfOrganizationRepository : IOrganizationRepository
    {
        private EfDbContext _dbContext = new EfDbContext();

        public IQueryable<Organization> Organizations
        {
            get { return _dbContext.Organizations; }
        }


        public Organization GetById(int id)
        {
            return Organizations.SingleOrDefault(x => x.Id.Equals(id));
        }


        public IQueryable<Organization> Companies
        {
            get { return Organizations; }
        }

        public IList<Organization> GetByParentId(int parentOrganizationId)
        {
            return GetByParentId(parentOrganizationId, false);
        }

        public IList<Organization> GetByParentId(int parentOrganizationId, bool includeParent)
        {
            // Returns all items which belongs to parent and parent itself
            if(includeParent)
            {
                return Organizations
                    .Where(x => x.StatusId == (byte)Status.Approved)
                    .Where(x => x.ParentId.Equals(parentOrganizationId) || x.Id.Equals(parentOrganizationId))
                    .ToList();
            }

            return Organizations
                .Where(x => x.StatusId == (byte)Status.Approved)
                .Where(x => x.ParentId.Equals(parentOrganizationId))
                .ToList();
        }

        public IList<MembershipUser> GetUsersInOrganization(int organizationId)
        {
            return null;
        }

        public void Save(Organization model)
        {

        }

        public void SubmitChanges()
        {
            
        }

    }
}
