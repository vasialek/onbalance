using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq;
using System.Configuration;

namespace OnBalance.Models
{
    public class SizeConvertorRepository
    {

        protected SizeConvertorDataContext _db = new SizeConvertorDataContext();
        //protected static List<SizeConvertor> _db = new List<SizeConvertor>();

        protected readonly static List<Category> _categories = new List<Category> { new Category { Id = 1, Name = "Vyr. batai" }, new Category { Id = 2, Name = "Mot. batai" }, new Category { Id = 3, Name = "Vaik. batai" } };

        //public SizeConvertorRepository()
        //    : base(ConfigurationManager.ConnectionStrings["OnlineBalanceConnectionString"].ToString())
        //{

        //}

        public List<SizeConvertor> GetAll(int sizeCategoryId)
        {
            //var db = new SizeConvertorDataContext();
            if( sizeCategoryId > 0 )
            {
                return _db.SizeConvertors.Where(x => x.category_id == sizeCategoryId).ToList();
            }

            return _db.SizeConvertors.ToList();
        }

        public IList<Category> GetCategories()
        {
            return _categories;
        }

        public void Update(IList<SizeConvertor> model)
        {
            if( model != null )
            {
                //var db = new SizeConvertorDataContext();
                int categoryId = model.FirstOrDefault().category_id;
                var deleted = _db.SizeConvertors.Where(x => x.category_id == categoryId);
                _db.SizeConvertors.DeleteAllOnSubmit(deleted);

                foreach(var item in model)
                {
                    _db.SizeConvertors.InsertOnSubmit(item);
                }

                _db.SubmitChanges();
            }
        }

        public void Insert(SizeConvertor newSize)
        {
            //var db = new SizeConvertorDataContext();
            _db.SizeConvertors.InsertOnSubmit(newSize);
            _db.SubmitChanges();
        }

        public void Delete(SizeConvertor model)
        {
            _db.SizeConvertors.DeleteOnSubmit(model);
            _db.SubmitChanges();
        }

        public SizeConvertor GetById(int id)
        {
            return _db.SizeConvertors.SingleOrDefault(x => x.id == id);
        }
    }
}
