using System;
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

        public IQueryable<Category> Categories
        {
            get
            {
                return _dbContext.Categories;
                //return Products.Select(x => new Category { Id = x.CategoryId });
            }
        }


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

        public ProductDetail GetDetailsById(int productDetailsId)
        {
            return _dbContext.ProductDetails.FirstOrDefault(x => x.Id == productDetailsId);
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
            var existing = Products.First(x => x.Id == product.Id);
            existing.InternalCode = product.InternalCode;
            existing.Name = product.Name;
            existing.PosId = product.PosId;
            existing.Price = product.Price;
            existing.StatusId = product.StatusId;
            existing.Uid = product.Uid;
            existing.UserId = product.UserId;
        }

        public void Update(ProductDetail pd)
        {
            var existing = _dbContext.ProductDetails.First(x => x.Id == pd.Id);
            existing.ParameterName = pd.ParameterName;
            existing.ParameterValue = pd.ParameterValue;
            existing.PriceMinor = pd.PriceMinor;
            existing.PriceReleaseMinor = pd.PriceReleaseMinor;
            existing.ProductId = pd.ProductId;
            existing.Quantity = pd.Quantity;
            existing.StatusId = pd.StatusId;
            existing.UpdatedAt = DateTime.UtcNow;
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

        public void Delete(Product p)
        {
            _dbContext.Products.Remove(p);
        }
    }
}
