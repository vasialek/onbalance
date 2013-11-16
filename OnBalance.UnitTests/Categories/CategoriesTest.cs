using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Moq;
using OnBalance.Domain.Abstract;
using OnBalance.Domain.Entities;
using OnBalance.Controllers;
using OnBalance.ViewModels.Categories;

namespace OnBalance.UnitTests.Categories
{
    [TestFixture]
    public class CategoriesTest
    {

        [Test]
        public void Can_Paginate()
        {
            var mock = new Mock<ICategoryRepository>();
            mock.Setup(x => x.Categories)
                .Returns(new List<Category> {
                    new Category { Id = 2, Name = "Light alcohol", PosId = 1, ParentId = 0, StatusId = (byte)OnBalance.Domain.Primitives.Status.Approved },
                    new Category { Id = 2, Name = "Food", PosId = 1, ParentId = 0, StatusId = (byte)OnBalance.Domain.Primitives.Status.Approved },
                    new Category { Id = 3, Name = "Men shoes", PosId = 1, ParentId = 0, StatusId = (byte)OnBalance.Domain.Primitives.Status.Approved }
                }.AsQueryable());

            CategoryController controller = new CategoryController(mock.Object);
            controller.PageSize = 2;


            var result = (PosCategoriesListViewModel)controller.List(1).Model;

            Assert.AreEqual(result.Categories.Count, 2);
        }

    }
}

