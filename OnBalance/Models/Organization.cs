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

        protected Organization _parent = null;

        partial void OnCreated()
        {
            _StatusId = (byte)Status.Pending;
            _CreatedAt = DateTime.Now;
            _parentId = 0;
        }

        /// <summary>
        /// Configuration of Organization/POS
        /// </summary>
        public OrganizationConfig Configuration { get { return new OrganizationConfig(); } }


        /// <summary>
        /// Gets parent Organization or null
        /// </summary>
        public Organization Parent
        {
            get
            {
                if(_parentId < 1)
                {
                    return null;
                }
                if(_parent == null)
                {
                    _parent = new OrganizationRepository().GetById(_parentId);
                }

                return _parent;
            }
        }

        /// <summary>
        /// Gets list of all children
        /// </summary>
        public IList<Organization> Children
        {
            get
            {
                return new OrganizationRepository().GetByParentId(Id);
            }
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