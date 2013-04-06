using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnBalance.Models
{
    public class CategoryStructure : BaseModel
    {

        public int Id { get; set; }

        public int Priority { get; set; }

        public byte StatusId { get; set; }

        public int CategoryId { get; set; }

        public string Name { get; set; }

    }

    public class CategoryStructureRepository
    {
        /// <summary>
        /// Simulate DB
        /// </summary>
        protected static List<CategoryStructure> _db = new List<CategoryStructure>();

        public CategoryStructure[] GetStructure(int categoryId)
        {
            var q = _db.Where(x => x.CategoryId == categoryId);
            return q == null ? null : q.ToArray();
        }

        public void Save(CategoryStructure model)
        {
            _db.Add(model);
        }

        public void Add(CategoryStructure model)
        {
            model.Id = model.Id > 0 ? model.Id : _db.Count + 1;
            _db.Add(model);
        }

        public void Update(CategoryStructure model)
        {
            CategoryStructure cs = _db.SingleOrDefault(x => x.Id == model.Id);
            if(cs != null)
            {
                cs.Name = model.Name;
                cs.StatusId = model.StatusId;
            }
        }

        public void SubmitChanges()
        {
            
        }

    }
}
