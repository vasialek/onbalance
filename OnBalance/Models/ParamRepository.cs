using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnBalance.Models
{
    public class ParamRepository : BaseRepository
    {

        public IList<ParamModel> Items
        {
            get
            {
                return new List<ParamModel>
                {
                    new ParamModel{ Id = 1001, Name = "Size" },
                    new ParamModel{ Id = 1002, Name = "Colour" },
                };
            }
        }

    }
}
