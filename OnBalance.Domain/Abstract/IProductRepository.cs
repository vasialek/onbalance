using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OnBalance.Domain.Entities;

namespace OnBalance.Domain.Abstract
{
    public interface IProductRepository
    {

        IQueryable<Product> Products { get; }

        IQueryable<Category> Categories { get; }

        void Save(Category category);

        void Save(Product product);

        void Save(ProductDetail productDetail);

        Category GetCategory(int id);

        Product GetById(int id);

        IList<ProductDetail> GetDetailsByProduct(int productId);

        void SubmitChanges();

        IEnumerable<Product> GetLastInPos(int posId, int p, int p_2);

        string[] GetAvailableSizes(int p);

        void Update(Product product);

        Product GetByUid(string id);
    }
}
