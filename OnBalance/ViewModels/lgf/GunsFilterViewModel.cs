using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using OnBalance.Models;

namespace OnBalance.ViewModels.lgf
{

    public class GunsFilterViewModel : IFilter
    {

        public string SearchBy { get; set; }

        public int[] SelectedGunProducer { get; set; }

        [Display(Name = "Producer name")]
        public IList<SelectListItem> GunProducers { get; set; }

        [Display(Name = "Type of gun")]
        public IList<string> GunsTypes { get; set; }

        public IList<SelectListItem> GunsTypesSelectItems
        {
            get
            {
                return GunsTypes.Select(x => new SelectListItem { Text = x, Value = x }).ToList();
            }
        }

        public bool IsAscending { get; set; }

        public string SortByField { get; set; }

        public int Offset{ get; set; }

        public int Limit { get; set; }

        public GunsFilterViewModel(IFilter filter)
        {

        }
    }
}
