using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;

namespace OnBalance.Models
{

    public enum ApiResponseCodes : int
    {
        /// <summary>
        /// Ok = 200
        /// </summary>
        Ok = HttpStatusCode.OK,
        /// <summary>
        /// NotFound = 404
        /// </summary>
        NotFound = HttpStatusCode.NotFound,
        /// <summary>
        /// BadRequest = 400
        /// </summary>
        BadRequest = HttpStatusCode.BadRequest
    }

}
