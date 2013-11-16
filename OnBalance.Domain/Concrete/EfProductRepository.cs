using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OnBalance.Domain.Abstract;
using OnBalance.Domain.Entities;

namespace OnBalance.Domain.Concrete
{
    class EfProductRepository : IProductRepository
    {
        private EfDbContext _dbContext = new EfDbContext();

        public IQueryable<Product> Products
        {
            get { return _dbContext.Products; }
        }
    }
}
