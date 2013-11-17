using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OnBalance.Domain.Entities;
using OnBalance.Controllers;
using Moq;
using OnBalance.Domain.Abstract;

namespace OnBalance.UnitTests.Organizations
{
    [TestFixture]
    public class OrganizationTest
    {
        //[SetUp]
        //public void Init()
        //{
        //}

        [Test]
        public void Test_Root_Organizations()
        {
            var organizations = TestOrganizationRepository.NewCollection();
            var organizationRepository = new Mock<IOrganizationRepository>();
            organizationRepository.Setup(x => x.Organizations)
                .Returns(organizations.AsQueryable());
            int totalRootOrganizations = organizations.Count(
                x => x.ParentId.Equals(0) 
                    && x.StatusId.Equals((byte)Status.Approved));

            var r = (IEnumerable<Organization>)new OrganizationController(organizationRepository.Object).List(null).Model;
            
            Assert.AreEqual(totalRootOrganizations, r.Count());
        }

        [Test]
        public void Test_Organizations_For_Pos()
        {
            int parentId = 2;
            var organizations = TestOrganizationRepository.NewCollection();
            var organizationRepository = new Mock<IOrganizationRepository>();
            organizationRepository.Setup(x => x.Organizations)
                .Returns(organizations.AsQueryable());
            //organizationRepository.Setup(x => x.GetByParentId(parentId, false))
            //    .Returns(organizations.ToList());
            int totalRootOrganizations = organizations.Count(
                x => (x.ParentId.Equals(parentId) || x.Id.Equals(parentId))
                    && x.StatusId.Equals((byte)Status.Approved));

            var r = (IEnumerable<Organization>)new OrganizationController(organizationRepository.Object).List(parentId).Model;

            Assert.AreEqual(totalRootOrganizations, r.Count());
        }
    }
}
