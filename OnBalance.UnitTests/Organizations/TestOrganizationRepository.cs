using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OnBalance.Domain.Entities;
using OnBalance.Domain.Abstract;
using Moq;

namespace OnBalance.UnitTests.Organizations
{

    class TestOrganizationRepository
    {
        private Mock<IOrganizationRepository> _organizationRepository = null;

        public static IList<Organization> NewCollection()
        {
            return new List<Organization> {
                new Organization{ Id = 1, Name = "Pos #1", StatusId = (byte)Status.Approved, ParentId = 0, CreatedAt = DateTime.Now },
                new Organization{ Id = 2, Name = "Pos #2", StatusId = (byte)Status.Approved, ParentId = 0, CreatedAt = DateTime.Now },
                new Organization{ Id = 3, Name = "Removed pos", StatusId = (byte)Status.Deleted, ParentId = 0, CreatedAt = DateTime.Now },
                new Organization{ Id = 4, Name = "Pos #2 child #1", StatusId = (byte)Status.Approved, ParentId = 1, CreatedAt = DateTime.Now },
                new Organization{ Id = 5, Name = "Pos #2 child #2", StatusId = (byte)Status.Approved, ParentId = 1, CreatedAt = DateTime.Now },
            };
        }

    }
}
