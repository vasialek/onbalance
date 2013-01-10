using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq;
using System.Configuration;

namespace OnBalance.Models
{
    public class BalanceItemRepository /*: BaseRepository*/
    {
        DataContext _dataContext = new DataContext(ConfigurationManager.ConnectionStrings["OnlineBalanceConnectionString"].ToString());

        //public BalanceItemRepository()
        //    : base(ConfigurationManager.ConnectionStrings["OnlineBalanceConnectionString"].ToString())
        //{
        //    _balanceItems = new List<BalanceItem>();
        //    _balanceItems.Add(new BalanceItem
        //    {
        //        Id = 10001
        //        , StatusId = Status.Approved
        //        , InternalCode = "123456qwerty"
        //        , IsNew = true
        //        , PosId = 1
        //        , Price = 6500
        //        , ProductName = "Test import"
        //        , Quantity = 3
        //    });
        //}

        public IQueryable<BalanceItem> Items
        {
            get
            {
                return _dataContext.GetTable<BalanceItem>();
            }
        }

        public IList<BalanceItem> GetLastUpdated()
        {
            return GetLastUpdated(0, 50);
        }

        /// <summary>
        /// Returns list of BalanceItem which has status Approved, order by ID desc.
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        public IList<BalanceItem> GetLastUpdated(int offset, int limit)
        {
            return Items.OrderByDescending(x => x.Id)
                .Skip(offset)
                .Take(limit)
                .ToList();
        }

        /// <summary>
        /// Inserts/updates item to DB and submits changes
        /// </summary>
        /// <param name="bi"></param>
        public void Save(BalanceItem bi)
        {

            try
            {
                var db = _dataContext.GetTable<BalanceItem>();
                BalanceItem entity = db.SingleOrDefault<BalanceItem>(x => x.InternalCode == bi.InternalCode);
                //Log.DebugFormat("BalanceItem with code {0} already exists...", bi.InternalCode);
                if(entity == null)
                {
                    bi.Status = Status.Approved;
                    db.InsertOnSubmit(bi);
                } else
                {
                    entity.Price = bi.Price;
                    entity.Quantity = bi.Quantity;
                    entity.ProductName = bi.ProductName;
                }
                db.Context.SubmitChanges();
            } catch(Exception ex)
            {
                //Log.Error("Error saving BalanceItem!", ex);
                throw ex;
            }
        }
    }
}
