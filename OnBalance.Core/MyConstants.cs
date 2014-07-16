using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnBalance.Core
{

    /// <summary>
    /// List of available status 
    /// </summary>
    public enum Status { Approved = 1, Deleted, Pending, Completed, Unknown, Failed }

    public class MyConstants
    {

        public const string Name = "Online Balance System";

        public const string Version = "0.4.10";

        public static string NameVersion
        {
            get { return string.Format("{0} (v{1})", Name, Version); }
        }

    }
}
