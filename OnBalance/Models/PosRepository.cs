using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnBalance.Models
{
    public class PosRepository
    {

        /// <summary>
        /// Gets list of all POS
        /// </summary>
        public IQueryable<Pos> Items
        {
            get
            {
                return new List<Pos> 
                {
                    new Pos{ Id = 100001, Name = "GJ Sportland.com", UserId = "GJ" }
                    , new Pos{ Id = 100002, Name = "GJ London", UserId = "GJ" }
                    , new Pos{ Id = 100005, Name = "GJ Gariunai", UserId = "GJ" }
                }.AsQueryable();
            }
        }

    }
}
