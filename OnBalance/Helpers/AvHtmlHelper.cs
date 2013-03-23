using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc.Html;
using OnBalance.Helpers;
using OnBalance;
using System.Globalization;

namespace System.Web.Mvc
{

    /// <summary>
    /// Extends MVC html helper
    /// </summary>
    public static class AvHtmlHelper
    {

        /// <summary>
        /// Returns string with A tag containing IMG
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="imageSrc">Full path to image</param>
        /// <param name="alt">Alternative text for image</param>
        /// <param name="hrefLink">Link for A href</param>
        /// <param name="htmlImageAttributes">Object with image attributes</param>
        /// <param name="htmlUrlAttributes">Object with attributes for URL</param>
        public static HtmlString ImageLink(this HtmlHelper htmlHelper, string imageSrc, string alt, string hrefLink, object htmlImageAttributes, object htmlUrlAttributes)
        {
            UrlHelper urlHelper = ((Controller)htmlHelper.ViewContext.Controller).Url;
            TagBuilder img = new TagBuilder("img");
            // Merge provided image attributes (in object) with image attributes
            var attributes = Common.DynamicObjectToDictionaryInsensitive(htmlImageAttributes);
            if( (attributes != null) && !attributes.Keys.Contains("title"))
            {
                attributes["title"] = alt;
            }
            img.MergeAttributes(attributes, true);
            img.MergeAttribute("src", imageSrc, true);
            img.MergeAttribute("alt", alt, true);

            TagBuilder urlTag = new TagBuilder("a");
            urlTag.MergeAttributes(Common.DynamicObjectToDictionary(htmlUrlAttributes), true);
            urlTag.MergeAttribute("href", hrefLink);
            urlTag.InnerHtml = img.ToString();

            return new HtmlString(urlTag.ToString());
        }

        public static HtmlString ImageLink(this HtmlHelper htmlHelper, string imageSrc, string alt, string action, string controller, object htmlImageAttributes, object routeValues, object htmlUrlAttributes)
        {
            UrlHelper urlHelper = ((Controller)htmlHelper.ViewContext.Controller).Url;
            return ImageLink(htmlHelper, imageSrc, alt, urlHelper.Action(action, controller, routeValues), htmlImageAttributes, htmlUrlAttributes);
        }

        public static HtmlString StatusDropDownList(this HtmlHelper htmlHelper, Status selectedValue)
        {
            return StatusDropDownList(htmlHelper, selectedValue, "status_id");
        }

        /// <summary>
        /// Returns dropdown list for available statuses
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="selectedValue"></param>
        /// <param name="name"></param>
        public static HtmlString StatusDropDownList(this HtmlHelper htmlHelper, Status selectedValue, string name)
        {
            var statuses = from x in Enum.GetValues(typeof(Status)).Cast<Status>()
                           select new SelectListItem
                           {
                               Text = x.ToString(),
                               Value = ((int)x).ToString(),
                               Selected = x.Equals(selectedValue)
                           };
            return htmlHelper.DropDownList(name, statuses);
        }

        public static HtmlString EnumDropDownList<TEnum>(this HtmlHelper htmlHelper, string name, TEnum selectedValue)
        {
            IEnumerable<TEnum> enums = Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
            var items = from x in enums
                        select new SelectListItem
                        {
                            Text = x.ToString(),
                            Value = x.ToString(),
                            Selected = x.Equals(selectedValue)
                        };

            return htmlHelper.DropDownList(name, items);
        }

        private static readonly string _scriptManagerKey = "SimpleScriptManagerKey";
        public static SimpleScriptManager ScriptManager(this HtmlHelper htmlHelper)
        {
            var sm = htmlHelper.ViewData[_scriptManagerKey] as SimpleScriptManager;
            if( sm == null )
            {
                sm = new SimpleScriptManager(htmlHelper);
                htmlHelper.ViewData[_scriptManagerKey] = sm;
            }

            return sm;
        }
    }
}
