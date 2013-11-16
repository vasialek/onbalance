using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OnBalance.Domain.Abstract;

namespace OnBalance.Domain.Concrete
{
    public class EfCategoryRepository : ICategoryRepository
    {
        private EfDbContext _dbContext = new EfDbContext();

        public IQueryable<Entities.Category> Categories
        {
            get { return _dbContext.Categories; }
        }
    }
}
