﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq;
using System.Configuration;

namespace OnBalance.Models
{
    public class BalanceItemRepository : BaseRepository
    {

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
                var db = GetTable<BalanceItem>();
                return db.Where(x => x.StatusId == (byte)Status.Approved);
            }
        }

        public IList<BalanceItem> GetLastUpdated()
        {
            return Items.ToList();
        }

        public void Save(BalanceItem bi)
        {

            try
            {
                var db = GetTable<BalanceItem>();
                BalanceItem entity = db.SingleOrDefault<BalanceItem>(x => x.InternalCode == bi.InternalCode);
                Log.DebugFormat("BalanceItem with code {0} already exists...", bi.InternalCode);
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
                Log.Error("Error saving BalanceItem!", ex);
                throw ex;
            }
        }
    }
}