using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace OnBalance.Models
{
    public class AjaxResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public string HtmlData { get; set; }
        public IList<string> Errors { get; set; }

        public AjaxResponse()
        {
            Status = false;
            Errors = new List<string>();
        }

    }
}
