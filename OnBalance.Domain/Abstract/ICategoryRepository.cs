using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OnBalance.Domain.Entities;

namespace OnBalance.Domain.Abstract
{

    public interface ICategoryRepository
    {
        IQueryable<Category> Categories { get; }

        IEnumerable<Category> GetCategoriesBy(int organizationId, int parentId, int offset, int limit);

        Category GetCategory(int id);

        void SubmitChanges();

        Category Save(Category category);

        void Add(CategoryStructure newItem);

        void Update(CategoryStructure item);
    }

}
