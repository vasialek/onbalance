using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace OnBalance.ViewModels.Parsers
{
    public class ParserOptions
    {
        [Display(Name = "Allow empty price")]
        public bool AllowEmptyPrice { get; set; }

        [Display(Name = "Redirect to errors")]
        public bool RedirectToErrors { get; set; }

        [Display(Name = "Redirect to not found categories")]
        public bool RedirectToCategories { get; set; }

        [Display(Name = "Prepare SQL to inser")]
        public bool PrepareInsertSql { get; set; }

    }
}
