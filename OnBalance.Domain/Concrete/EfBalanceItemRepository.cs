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
            if (item.Id < 1)
            {
                _dbContext.BalanceItems.Add(item);
            }
            else
            {
                var existing = _dbContext.BalanceItems.First(x => x.Id == item.Id);
                existing.ChangedFrom = item.ChangedFrom;
                existing.InternalCode = item.InternalCode;
                existing.PosId = item.PosId;
                existing.PriceOfRelease = item.PriceOfRelease;
                existing.Price = item.Price;
                existing.ProductName = item.ProductName;
                existing.Quantity = item.Quantity;
                existing.SizeName = item.SizeName;
                existing.StatusId = item.StatusId;
            }
        }

        public void SubmitChanges()
        {
            _dbContext.SaveChanges();
        }

        public void Delete(Entities.BalanceItem item)
        {
            _dbContext.BalanceItems.Remove(item);
        }
    }
}
