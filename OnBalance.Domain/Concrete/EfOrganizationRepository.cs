using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OnBalance.Domain.Abstract;
using OnBalance.Domain.Entities;
using System.Web.Security;

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

        public void Save(Organization model)
        {
            
        }

        public IList<Organization> GetByParentId(int parentOrganizationId)
        {
            return Organizations.ToList();
        }

        public IList<MembershipUser> GetUsersInOrganization(int organizationId)
        {
            return null;
        }

        public void SubmitChanges()
        {
            
        }


        public IList<Organization> GetByParentId(int p, bool p_2)
        {
            throw new NotImplementedException();
        }
    }
}
