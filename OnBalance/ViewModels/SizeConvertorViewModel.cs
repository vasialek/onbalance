using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnBalance.Models;

namespace OnBalance.ViewModels
{
    public class SizeConvertorViewModel
    {

        public Category SelectedCategory { get; set; }

        public IList<Category> Categories { get; set; }

        public IList<SizeConvertor> Sizes { get; set; }

    }
}