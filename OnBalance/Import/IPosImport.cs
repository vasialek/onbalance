using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnBalance.Models;

namespace OnBalance.Import
{
    public interface IPosImport
    {

        /// <summary>
        /// Returns whether POS has any new/updated items
        /// </summary>
        bool IsPosUpdated();

        /// <summary>
        /// Returns from POS when it was updated
        /// </summary>
        DateTime GetPosUpdatedAt();

        /// <summary>
        /// Makes importer to look for new products in POS
        /// </summary>
        /// <returns></returns>
        bool CheckNewProducts();

        /// <summary>
        /// Reads (downloads) all new/updated items from POS
        /// </summary>
        void GetData();

        /// <summary>
        /// Returns list of all new products (if there are any)
        /// </summary>
        /// <returns></returns>
        IList<BalanceItem> GetNewProducts();
    }
}
