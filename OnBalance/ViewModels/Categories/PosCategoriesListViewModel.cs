using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnBalance.Domain.Entities;

namespace OnBalance.ViewModels.Categories
{
    public class PosCategoriesListViewModel
    {
        public Organization Organization { get; set; }

        public IList<Category> Categories { get; set; }
    }
}
