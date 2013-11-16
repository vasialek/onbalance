using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnBalance.Domain.Entities;

namespace OnBalance.ViewModels.Categories
{
    public class CategoryStructureViewModel
    {
        protected IList<CategoryStructure> _categoryStructure = null;

        public Category Category { get; set; }

        public List<CategoryType> CategoryTypes
        {
            get { return new CategoryTypeRepository().Items.ToList(); }
        }

        public IList<CategoryStructure> CategoryStructure
        {
            get
            {
                if(_categoryStructure == null)
                {
                    _categoryStructure = new List<CategoryStructure>();
                }
                return _categoryStructure;
            }
            set
            {
                _categoryStructure = value;
            }
        }

    }
}
