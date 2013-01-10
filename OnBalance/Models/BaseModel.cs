using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnBalance.Models
{

    /// <summary>
    /// Decorates model with some useful methods/properties
    /// </summary>
    public class BaseModel
    {

        public int StatusId = 0;

        public string StatusName
        {
            get
            {
                IQueryable<Status> statuses = Enum.GetValues(typeof(Status)).AsQueryable().Cast<Status>();
                string name = statuses.SingleOrDefault(x => (int)x == StatusId).ToString();
                return name ?? StatusId.ToString();
            }
        }

    }
}
