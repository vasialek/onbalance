using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnBalance.Models
{
    public class CategoryListItem
    {

        public string Name { get; set; }

        public CategoryListItem(OnBalance.Domain.Entities.Category model)
        {

        }

    }
}
