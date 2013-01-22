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

        protected readonly static List<Category> _categories = null; //new ProductDataContext().Categories.ToList();

        public List<SizeConvertor> GetAll(int sizeCategoryId)
        {
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
