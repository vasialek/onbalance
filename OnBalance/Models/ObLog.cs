using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq.Mapping;

namespace OnBalance.Models
{

    [Table(Name = "ob_log")]
    public class ObLog
    {

        [Column(Name = "Date")]
        public DateTime Date { get; set; }

        [Column(Name = "Level")]
        public string Level { get; set; }

        [Column(Name = "Message")]
        public string Message { get; set; }

        [Column]
        public string Exception { get; set; }
    }
}