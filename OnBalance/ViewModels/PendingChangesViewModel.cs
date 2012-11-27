using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnBalance.ViewModels
{
    public class PendingChangesViewModel
    {

        public int LocalNewRecordsQnt { get; set; }

        public int LocalChangedRecordsQnt { get; set; }

        public DateTime LastSyncing { get; set; }

        public int TotalSynced { get; set; }

    }
}
