using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using Moq;
using OnBalance.Domain.Abstract;
using OnBalance.Domain.Entities;
using OnBalance.Domain.Concrete;

namespace OnBalance.Infrastructure
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {

        private IKernel _ninjectKernel = null;

        public NinjectControllerFactory()
        {
            _ninjectKernel = new StandardKernel();
            AddBindings();
        }

        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            return controllerType == null ? null : (IController)_ninjectKernel.Get(controllerType);
        }

        private void AddBindings()
        {
            // Mock ProductRepository
            Mock<IProductRepository> productMock = new Mock<IProductRepository>();
            productMock.Setup(x => x.Products)
                .Returns(new List<Product> {
                    new Product { Id = 666001, CategoryId = 1, InternalCode = "internal_001", Name = "Beer", PosId = 1, Price = 10m, StatusId = (byte)Status.Approved, Uid = "uid_0001", UserId = "TestUser" },
                    new Product { Id = 666002, CategoryId = 2, InternalCode = "internal_002", Name = "Cheese", PosId = 1, Price = 15m, StatusId = (byte)Status.Approved, Uid = "uid_0002", UserId = "TestUser" },
                    new Product { Id = 666001, CategoryId = 1, InternalCode = "internal_003", Name = "Beer light", PosId = 1, Price = 5m, StatusId = (byte)Status.Approved, Uid = "uid_0003", UserId = "TestUser" }
                }.AsQueryable());
            _ninjectKernel.Bind<IProductRepository>().ToConstant(productMock.Object);

            // Mock CategoryRepository
            //var categoryMock = new Mock<ICategoryRepository>();
            //categoryMock.Setup(x => x.Categories)
            //    .Returns(new List<Category> {
            //        new Category { Id = 2, Name = "Light alcohol", PosId = 1, ParentId = 0, StatusId = (byte)OnBalance.Domain.Primitives.Status.Approved },
            //        new Category { Id = 2, Name = "Food", PosId = 1, ParentId = 0, StatusId = (byte)OnBalance.Domain.Primitives.Status.Approved },
            //        new Category { Id = 3, Name = "Men shoes", PosId = 1, ParentId = 0, StatusId = (byte)OnBalance.Domain.Primitives.Status.Approved }
            //    }.AsQueryable());
            //_ninjectKernel.Bind<ICategoryRepository>().ToConstant(categoryMock.Object);
            _ninjectKernel.Bind<ICategoryRepository>().To<EfCategoryRepository>();
        }

    }
}