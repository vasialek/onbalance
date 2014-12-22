using OnBalance.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnBalance.Domain.Abstract
{
    public interface IBalanceItemRepository
    {
        IQueryable<BalanceItem> BalanceItems { get; }
        void Save(BalanceItem item);
        void SubmitChanges();
        void Delete(BalanceItem item);
    }
}
