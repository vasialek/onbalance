using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnBalance.Domain.Entities;

namespace OnBalance.ViewModels
{

    // Using camelCase for properities, for JS

    public class BaseApiResponse
    {

        protected OnBalance.Models.ApiResponseCodes _responseCode;

        /// <summary>
        /// Like HTTP response code - 200, 404, etc..
        /// </summary>
        public string code { get { return ((int)_responseCode).ToString(); } }

        public string message { get; set; }

        public BaseApiResponse()
            : this(OnBalance.Models.ApiResponseCodes.Ok)
        {
        }

        public BaseApiResponse(OnBalance.Models.ApiResponseCodes responseCode)
        {
            SetResponseCode(responseCode);
        }

        public void SetResponseCode(OnBalance.Models.ApiResponseCodes responseCode)
        {
            _responseCode = responseCode;
            message = "";
        }
    }

    public class ApiProductsListResponse : BaseApiResponse
    {
        public int total { get { return products == null ? 0 : products.Count; } }

        public IList<Product> products
        {
            get;
            set;
        }
    }

    public class ApiPosListReponse : BaseApiResponse
    {
        public int total { get { return listOfPos == null ? 0 : listOfPos.Count; } }

        public IList<Organization> listOfPos
        {
            get;
            set;
        }
    }

    public class ApiCategoriesListReponse : BaseApiResponse
    {
        public int total { get { return categories == null ? 0 : categories.Count; } }

        public IList<Category> categories
        {
            get;
            set;
        }
    }
}
