﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace OnBalance.Models
{
    public class ProductRepository : BaseRepository
    {

        protected ProductDataContext _dataContext = new ProductDataContext(ConfigurationManager.ConnectionStrings["OnlineBalanceConnectionString"].ToString());

        /// <summary>
        /// Little cache for available parameter names
        /// </summary>
        protected static Dictionary<string, string[]> _arNames = new Dictionary<string, string[]>();


        public IQueryable<Product> Items
        {
            get { return _dataContext.Products; }
        }

        public static string[] GetAvailableNames(string parameterName)
        {
            return new string[] { "35", "36", "37", "38", "39", "40", "41", "42", "42.5", "43", "44", "44.5", "45", "46", "46.5", "47", "48", "49", "49.5", "50", "51", "52", "53", "54", "54.5", "55" };
        }

        /// <summary>
        /// Returns Product by ID or null
        /// </summary>
        /// <param name="id"></param>
        public Product GetById(int id)
        {
            return Items.SingleOrDefault(x => x.id == id);
        }


        public void Update(Product model)
        {
            var p = Items.SingleOrDefault(x => x.id == model.id);
            p.name = model.name;
            _dataContext.SubmitChanges();
        }

        public void Save(Product model)
        {
            _dataContext.Products.InsertOnSubmit(model);
            _dataContext.SubmitChanges();
        }

        /// <summary>
        /// Returns list of products (not deleted)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="offset"></param>
        /// <param name="perPage"></param>
        public IQueryable<Product> GetLastInPos(int id, int offset, int perPage)
        {
            return _dataContext.GetTable<Product>()
                .Where(x => x.pos_id == id && x.status_id != (byte)Status.Deleted)
                .OrderBy(x => x.id)
                .Skip(offset)
                .Take(perPage);
        }
    }
}