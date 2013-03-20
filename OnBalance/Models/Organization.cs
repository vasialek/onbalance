using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq.Mapping;
using System.Web.Mvc;

namespace OnBalance.Models
{

    partial class Organization
    {

        partial void OnCreated()
        {
            _StatusId = (byte)Status.Pending;
            _CreatedAt = DateTime.Now;
            _parentId = 0;
        }

        /// <summary>
        /// Gets name of status
        /// </summary>
        public string StatusName
        {
            get
            {
                Status s = Status.Unknown;
                Enum.TryParse(StatusId.ToString(), out s);
                return s.ToString();
            }
        }

    }
}