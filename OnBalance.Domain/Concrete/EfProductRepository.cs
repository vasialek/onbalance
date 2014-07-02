﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OnBalance.Domain.Abstract;
using OnBalance.Domain.Entities;
using OnBalance.Domain.Primitives;

namespace OnBalance.Domain.Concrete
{
    public class EfProductRepository : IProductRepository
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

        public void Save(Product product)
        {
            _dbContext.Products.Add(product);
        }

        public void Save(ProductDetail productDetail)
        {
            _dbContext.ProductDetails.Add(productDetail);
        }

        public Category GetCategory(int id)
        {
            throw new NotImplementedException();
        }

        public Product GetById(int id)
        {
            return Products.FirstOrDefault(x => x.Id == id);
        }

        public void SubmitChanges()
        {
            _dbContext.SaveChanges();
        }

        public IEnumerable<Product> GetLastInPos(int posId, int p, int p_2)
        {
            return Products
                .Where(x => x.PosId == posId && x.StatusId != (byte)Status.Deleted)
                .OrderByDescending(x => x.Id)
                .Skip(p)
                .Take(p_2);
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

        public IList<ProductDetail> GetDetailsByProduct(int productId)
        {
            return _dbContext
                .ProductDetails
                .Where(x => x.ProductId == productId)
                .ToList();
            
        }
    }
}
