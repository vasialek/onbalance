using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnBalance.Models
{
    public class CategoryRepository : BaseRepository
    {

        /// <summary>
        /// Simulate DB
        /// </summary>
        protected static List<Category> _db = new List<Category>();

        public IQueryable<Category> Items { get { return _dataContext.GetTable<Category>(); } }


        internal IList<Category> GetCategoriesBy(int organizationId, int parentId, int offset, int limit)
        {
            return Items
                .Where(x => x.OrganizationId == organizationId && (x.ParentId == parentId || x.Id == parentId) /*&& x.StatusId == (byte)Status.Approved*/)
                .Distinct()
                .OrderBy(x => x.Id)
                .Skip(offset)
                .Take(limit)
                .ToList();
        }

    }
}
