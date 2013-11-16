using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OnBalance.Domain.Abstract;
using OnBalance.Domain.Entities;

namespace OnBalance.Domain.Concrete
{
    public class EfCategoryRepository : ICategoryRepository
    {
        private EfDbContext _dbContext = new EfDbContext();

        public IQueryable<Category> Categories
        {
            get { return _dbContext.Categories; }
        }


        public IEnumerable<Category> GetCategoriesBy(int organizationId, int parentId, int offset, int limit)
        {
            return Categories
                //.Where(x => x.OrganizationId == organizationId)
                .Where(x => x.ParentId == parentId)
                .OrderBy(x => x.Name)
                .OrderBy(x => x.Id)
                .Skip(offset)
                .Take(limit);
        }



        public Category GetCategory(int id)
        {
            throw new NotImplementedException();
        }

        public void SubmitChanges()
        {
            throw new NotImplementedException();
        }

        public void Save(Category category)
        {
            throw new NotImplementedException();
        }


        Category ICategoryRepository.Save(Category category)
        {
            throw new NotImplementedException();
        }

        public void Add(CategoryStructure newItem)
        {
            throw new NotImplementedException();
        }


        public void Update(CategoryStructure item)
        {
            throw new NotImplementedException();
        }
    }
}
