using OnBalance.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnBalance.Domain.Concrete
{
    public class EfBalanceItemRepository : IBalanceItemRepository
    {

        private EfDbContext _dbContext = new EfDbContext();

        public IQueryable<Entities.BalanceItem> BalanceItems
        {
            get { return _dbContext.BalanceItems; }
        }

        public void Save(Entities.BalanceItem item)
        {
            _dbContext.BalanceItems.Add(item);
        }


        public void SubmitChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}
