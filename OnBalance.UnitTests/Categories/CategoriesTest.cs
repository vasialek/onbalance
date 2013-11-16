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
        protected Mock<ICategoryRepository> _categoryMock = null;
        protected Mock<IProductRepository> _productRepository = null;
        protected Mock<IOrganizationRepository> _organizationRepository = null;
        protected CategoryController _categoryController = null;
        
        protected IList<Organization> _organizations = null;
        protected IList<Product> _products = null;
        protected IList<Category> _categories = null;

        [SetUp]
        public void Init()
        {
            _organizations = new List<Organization> {
                new Organization{ Id = 1, Name = "Pos #1", StatusId = (byte)Status.Approved, ParentId = 0, CreatedAt = DateTime.Now },
                new Organization{ Id = 2, Name = "Pos #2", StatusId = (byte)Status.Approved, ParentId = 0, CreatedAt = DateTime.Now },
            };

            Organization pos1 = _organizations.OrderBy(x => x.Id).First();
            Organization pos2 = _organizations.OrderBy(x => x.Id).Skip(1).First();
            _categories = new List<Category> {
                new Category { Id = 2, Name = "Light alcohol", OrganizationId = pos1.Id, ParentId = 0, StatusId = (byte)Status.Approved, CategoryTypeId = 1 },
                new Category { Id = 2, Name = "Food", OrganizationId = pos1.Id, ParentId = 0, StatusId = (byte)Status.Approved, CategoryTypeId = 1 },
                new Category { Id = 3, Name = "Men shoes", OrganizationId = pos2.Id, ParentId = 0, StatusId = (byte)Status.Approved, CategoryTypeId = 1 },
                new Category { Id = 4, Name = "Women shoes", OrganizationId = pos2.Id, ParentId = 0, StatusId = (byte)Status.Approved, CategoryTypeId = 1 },
                new Category { Id = 5, Name = "Children shoes", OrganizationId = pos2.Id, ParentId = 0, StatusId = (byte)Status.Approved, CategoryTypeId = 1 }
            };

            Category lightAlcohol = _categories.First(x => x.Name.Contains("Light alcohol"));
            Category food = _categories.First(x => x.Name.Equals("Food"));
            _products = new List<Product> {
                    new Product { Id = 1, CategoryId = lightAlcohol.Id, InternalCode = "internal_001", Name = "Beer", PosId = pos1.Id, Price = 10m, StatusId = (byte)Status.Approved, Uid = "uid_0001", UserId = "TestUser", CreatedAt = DateTime.Now },
                    new Product { Id = 2, CategoryId = food.Id, InternalCode = "internal_002", Name = "Cheese", PosId = pos1.Id, Price = 15m, StatusId = (byte)Status.Approved, Uid = "uid_0002", UserId = "TestUser", CreatedAt = DateTime.Now },
                    new Product { Id = 3, CategoryId = lightAlcohol.Id, InternalCode = "internal_003", Name = "Beer light", PosId = pos1.Id, Price = 5m, StatusId = (byte)Status.Approved, Uid = "uid_0003", UserId = "TestUser", CreatedAt = DateTime.Now }
            };

            _organizationRepository = new Mock<IOrganizationRepository>();
            _organizationRepository.Setup(x => x.Organizations)
                .Returns(_organizations.AsQueryable());

            _categoryMock = new Mock<ICategoryRepository>();
            _categoryMock.Setup(x => x.Categories)
                .Returns(_categories.AsQueryable());

            _productRepository = new Mock<IProductRepository>();
            _productRepository.Setup(x => x.Products)
                .Returns(_products.AsQueryable());

            _categoryController = new CategoryController(
                _categoryMock.Object,
                _organizationRepository.Object,
                _productRepository.Object);
        }

        [Test]
        public void List_All_Categories()
        {
            _categoryController.PageSize = 20;
            var result = (PosCategoriesListViewModel)_categoryController.List(null).Model;

            Assert.AreEqual(result.Categories.Count, _categories.Count);
        }

        [Test]
        public void Categories_By_Organization()
        {
            int organizationId = 3;
            _categoryController.PageSize = 20;
            var r = (PosCategoriesListViewModel)_categoryController.List(organizationId).Model;

            Assert.AreEqual(_categories.Count(x => x.ParentId.Equals(0) && x.OrganizationId.Equals(organizationId)), r.Categories.Count);
        }
    }
}

