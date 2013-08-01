using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnBalance.Models
{
    public interface IFilter
    {

        string SearchBy { get; set; }

        bool IsAscending { get; set; }

        string SortByField { get; set; }

        public int Offset { get; set; }

        public int Limit { get; set; }

    }
}
