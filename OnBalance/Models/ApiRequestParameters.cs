using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnBalance.Models
{
    public class ApiRequestParameters
    {
        public int ParentId { get; set; }
        public int Offset { get; set; }
        public int Limit { get; set; }
        public string Sort { get; set; }

        public void SetLimitIfNotPositive(int v)
        {
            if(Limit < 1)
            {
                Limit = v;
            }
        }

        public override string ToString()
        {
            return string.Format("Offset: {0}, Limit: {1}, Sort: {2}", Offset, Limit, Sort);
        }
    }
}
