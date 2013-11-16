using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OnBalance.Domain.Primitives;

namespace OnBalance.Domain.Entities
{
    public class BaseModel
    {

        protected byte _statusId = 0;

        //[ScaffoldColumn(false)]
        public string StatusName
        {
            get
            {
                IQueryable<Status> statuses = Enum.GetValues(typeof(Status)).AsQueryable().Cast<Status>();
                string name = statuses.SingleOrDefault(x => (int)x == _statusId).ToString();
                return name ?? _statusId.ToString();
            }
        }

        //[ScaffoldColumn(false)]
        public virtual string StatusCssStyle
        {
            get
            {
                switch(_statusId)
                {
                    case (byte)Status.Approved:
                        return "label-success";
                    case (byte)Status.Deleted:
                        return "label-inverse";
                }
                return "";
            }
        }

    }
}
