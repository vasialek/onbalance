using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc.Html;
using OnBalance.Helpers;
using OnBalance;
using System.Globalization;
using OnBalance.Core;

namespace System.Web.Mvc
{

    /// <summary>
    /// Extends MVC html helper
    /// </summary>
    public static class AvHtmlHelper
    {

        public static HtmlString LoaderDiv(this HtmlHelper htmlHelper)
        {
            return LoaderDiv(htmlHelper, null);
        }

        public static HtmlString LoaderDiv(this HtmlHelper htmlHelper, object htmlAttributes)
        {
            TagBuilder div = new TagBuilder("div");
            div.MergeAttributes(Common.DynamicObjectToDictionary(new { id = "LoaderDiv", style = "display: none;" }));
            if(htmlAttributes != null)
            {
                div.MergeAttributes(Common.DynamicObjectToDictionary(htmlAttributes), true);
            }
            UrlHelper urlHelper = ((Controller)htmlHelper.ViewContext.Controller).Url;
            
            TagBuilder img = new TagBuilder("img");
            img.MergeAttributes(Common.DynamicObjectToDictionary(new { width = 16, height = 16, alt = MyMessages.ButtonText.Loading, src = urlHelper.Content("~/images/loader.gif") }));

            div.InnerHtml = img.ToString();
            return new HtmlString(div.ToString());
        }

        public static HtmlString Image(this HtmlHelper htmlHelper, string src, int w, int h, string alt, object htmlAttributes)
        {
            Dictionary<string, object> attributes = Common.DynamicObjectToDictionaryInsensitive(htmlAttributes);
            attributes["width"] = w;
            attributes["height"] = h;
            TagBuilder img = ImageTag(src, alt, attributes);
            return new HtmlString(img.ToString());
        }

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
            //UrlHelper urlHelper = ((Controller)htmlHelper.ViewContext.Controller).Url;
            TagBuilder img = new TagBuilder("img");
            // Merge provided image attributes (in object) with image attributes
            var attributes = Common.DynamicObjectToDictionaryInsensitive(htmlImageAttributes);
            if((attributes != null) && !attributes.Keys.Contains("title"))
            {
                attributes["title"] = alt;
            }
            img.MergeAttributes(attributes, true);
            img.MergeAttribute("src", imageSrc, true);
            img.MergeAttribute("alt", alt, true);

            TagBuilder urlTag = new TagBuilder("a");
            urlTag.MergeAttributes(Common.DynamicObjectToDictionary(htmlUrlAttributes), true);
            urlTag.MergeAttribute("href", hrefLink);
            //urlTag.InnerHtml = ImageTag(imageSrc, alt, htmlImageAttributes).ToString();
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

        public static HtmlResourcesAggregator ResourceManager(this HtmlHelper htmlHelper)
        {
            var sm = htmlHelper.ViewData["__HtmlResourcesAggregatorStorage"] as HtmlResourcesAggregator;
            if( sm == null )
            {
                sm = new HtmlResourcesAggregator(htmlHelper);
                htmlHelper.ViewData["__HtmlResourcesAggregatorStorage"] = sm;
            }

            return sm;
        }

        private static TagBuilder ImageTag(string src, string alt, object htmlAttributes)
        {
            Dictionary<string, object> attributes = Common.DynamicObjectToDictionaryInsensitive(htmlAttributes);
            return ImageTag(src, alt, attributes);
        }

        /// <summary>
        /// Helper method to create image tag
        /// </summary>
        /// <param name="src"></param>
        /// <param name="alt"></param>
        /// <param name="htmlAttributes"></param>
        private static TagBuilder ImageTag(string src, string alt, Dictionary<string, object> htmlAttributes)
        {
            TagBuilder img = new TagBuilder("img");
            // Merge provided image attributes (in object) with image attributes
            if(!htmlAttributes.Keys.Contains("title"))
            {
                htmlAttributes["title"] = alt;
            }
            img.MergeAttributes(htmlAttributes, true);
            img.MergeAttribute("src", src, true);
            img.MergeAttribute("alt", alt, true);

            return img;
        }
    }
}
