using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnBalance.Domain.Entities;

namespace OnBalance.ViewModels.Categories
{
    public class PosCategoryViewModel
    {

        public Organization Organization { get; set; }

        public Category Category { get; set; }
    }
}
