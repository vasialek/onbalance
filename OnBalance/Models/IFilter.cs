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

        int Offset { get; set; }

        int Limit { get; set; }

    }
}
