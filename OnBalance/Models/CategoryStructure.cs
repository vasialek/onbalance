using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnBalance.Models
{
    public class CategoryStructure : BaseModel
    {

        public enum FieldTypes : int { Int = 1, String, List };

        public int Id { get; set; }

        public int Priority { get; set; }

        public byte StatusId { get; set; }

        public int CategoryId { get; set; }

        /// <summary>
        /// Gets/sets whether this field is used for filtering
        /// </summary>
        public bool IsDimension { get; set; }

        public string FieldName { get; set; }

        public FieldTypes FieldType { get; set; }

        /// <summary>
        /// ID of list in case field type is List
        /// </summary>
        public object FieldValue { get; set; }

        public object FieldDefault { get; set; }

        public bool IsRequired { get; set; }

        /// <summary>
        /// In case data should be stored in I18N table
        /// </summary>
        public bool IsI18n { get; set; }

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
                cs.FieldName = model.FieldName;
                cs.StatusId = model.StatusId;
            }
        }

        public void SubmitChanges()
        {
            
        }

    }
}
