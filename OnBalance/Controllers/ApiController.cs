﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnBalance.Models;
using OnBalance.ViewModels;
using System.Net;

namespace OnBalance.Controllers
{
    public class ApiController : BaseController
    {

        public const string CURRENT_API_VERSION = "v1";

        //
        // GET: /api/

        public ActionResult Index()
        {
            return Content("API for OnBalance :)");
        }

        //
        // GET: /api/v1/pos/100001/products?offset=0&limit=10

        public ActionResult Pos(int? id, string items)
        {
            InfoFormat("API: /pos: ID: {0}, items: {1}", id, items);
            ApiRequestParameters rp = new ApiRequestParameters();
            TryUpdateModel(rp);
            InfoFormat("RequestParameters: offset: {0}, limit: {1}, sort: {2}", rp.Offset, rp.Limit, rp.Sort);

            BaseApiResponse resp = new BaseApiResponse(ApiResponseCodes.BadRequest);
            resp.message = "Unrecognized request, please specify what do you want.";
            switch(items)
            {
                case "products":
                    resp = GetProductsInPos(id.HasValue ? id.Value : 0, rp);
                    break;
                case "categories":
                    resp = GetCategoriesInPos(id.HasValue ? id.Value : 0, rp);
                    break;
                default:
                    if(id.HasValue)
                    {
                        rp.ParentId = id.Value;
                    }else
                    {
                        rp.ParentId = 0;
                    }
                    resp = GetListOfPos(rp);
                    break;
            }

            return Json(resp, JsonRequestBehavior.AllowGet);
        }

        protected BaseApiResponse GetCategoriesInPos(int posId, ApiRequestParameters rp)
        {
            ApiCategoriesListReponse resp = new ApiCategoriesListReponse();

            var db = new CategoryRepository();
            resp.categories = db.GetCategoriesBy(posId, 0, rp.Offset, rp.Limit);
            resp.SetResponseCode(ApiResponseCodes.Ok);

            return resp;
        }

        protected ApiPosListReponse GetListOfPos(ApiRequestParameters rp)
        {
            ApiPosListReponse resp = new ApiPosListReponse();

            rp.SetLimitIfNotPositive(10);
            //if(rp.ParentId > 0)
            //{
            //    resp.listOfPos = new OrganizationRepository().GetByParentId(rp.ParentId);
            //    resp.message = string.Concat("List of POS by parent ", rp.ParentId);
            //    InfoFormat("Got list of POS by parent ID: {0}, offset {1}, limit: {2}. Total POS are {3}", rp.ParentId, rp.Offset, rp.Limit, resp.total);
            //} else
            //{
            //    resp.listOfPos = new OrganizationRepository().GetListOfLastPos(rp.Offset, rp.Limit);
            //    InfoFormat("Got list of POS, offset {0}, limit: {1}. Total POS are {2}", rp.Offset, rp.Limit, resp.total);
            //}
            resp.listOfPos = new OrganizationRepository().GetByParentId(rp.ParentId, true);
            resp.message = string.Concat("List of POS by parent ", rp.ParentId);
            InfoFormat("Got list of POS by parent ID: {0}, offset {1}, limit: {2}. Total POS are {3}", rp.ParentId, rp.Offset, rp.Limit, resp.total);
            resp.SetResponseCode(ApiResponseCodes.Ok);
            return resp;
        }


        protected ApiProductsListResponse GetProductsInPos(int posId, ApiRequestParameters rp)
        {
            ApiProductsListResponse resp = new ApiProductsListResponse();
            if(posId < 1)
            {
                ErrorFormat("Bad ID of POS (ID: {0}) to retrieve products!", posId);
                resp.SetResponseCode(ApiResponseCodes.BadRequest);
            }

            var pos = new OrganizationRepository().GetById(posId);
            if(pos == null)
            {
                ErrorFormat("Could not retrieve products for non-existing POS (ID: {0})!", posId);
                resp.SetResponseCode(ApiResponseCodes.NotFound);
                resp.message = MyMessages.Products.PosIsNotFound;
                return resp;
            }

            rp.SetLimitIfNotPositive(10);
            InfoFormat("API: getting list of product in POS #{0}, parameters: {1}", posId, rp);
            var products = new ProductRepository().GetLastInPos(posId, rp.Offset, rp.Limit);
            resp.products = products.ToList();

            return resp;
        }
    }
}
