﻿using System;
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

        public IQueryable<Category> Categories { get { return Products.Select(x => new Category { Id = x.CategoryId }); } }


        public void Save(Category category)
        {
            throw new NotImplementedException();
        }


        public Category GetCategory(int id)
        {
            throw new NotImplementedException();
        }

        public Product GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void SubmitChanges()
        {
            
        }

        public IEnumerable<Product> GetLastInPos(int posId, int p, int p_2)
        {
            throw new NotImplementedException();
        }


        public string[] GetAvailableSizes(int p)
        {
            throw new NotImplementedException();
        }


        public void Update(Product product)
        {
            throw new NotImplementedException();
        }

        public Product GetByUid(string id)
        {
            throw new NotImplementedException();
        }
    }
}
