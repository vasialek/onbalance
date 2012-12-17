using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq;
using System.Configuration;

namespace OnBalance.Models
{
    public class SizeConvertorRepository : DataContext
    {

        //protected static List<SizeConvertor> _db = new List<SizeConvertor>();

        protected readonly static List<Category> _categories = new List<Category> { new Category { Id = 1, Name = "Vyr. batai" }, new Category { Id = 2, Name = "Mot. batai" }, new Category { Id = 3, Name = "Vaik. batai" } };

        public SizeConvertorRepository()
            : base(ConfigurationManager.ConnectionStrings["OnlineBalanceConnectionString"].ToString())
        {

        }

        public List<SizeConvertor> GetAll(int sizeCategoryId)
        {
            var db = new SizeConvertorDataContext();
            if( sizeCategoryId > 0 )
            {
                return db.SizeConvertors.Where(x => x.category_id == sizeCategoryId).ToList();
            }

            return db.SizeConvertors.ToList();
        }

        public IList<Category> GetCategories()
        {
            return _categories;
        }

        public void Update(IList<SizeConvertor> model)
        {
            if( model != null )
            {
                var db = new SizeConvertorDataContext();
                int categoryId = model.FirstOrDefault().category_id;
                var deleted = db.SizeConvertors.Where(x => x.category_id == categoryId);
                db.SizeConvertors.DeleteAllOnSubmit(deleted);

                foreach(var item in model)
                {
                    db.SizeConvertors.InsertOnSubmit(item);
                }

                db.SubmitChanges();
            }
        }

        public void Insert(SizeConvertor newSize)
        {
            var db = new SizeConvertorDataContext();
            db.SizeConvertors.InsertOnSubmit(newSize);
            db.SubmitChanges();
        }
    }
}