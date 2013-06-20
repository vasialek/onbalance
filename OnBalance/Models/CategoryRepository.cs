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

        public IList<Category> Items { get { return _db.ToList(); } }

    }
}
