using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Diagnostics;

namespace OnBalance.Helpers
{

    /// <summary>
    /// HTML resources/files (JS and CSS) aggregator. 
    /// Based on SimpleScriptManager
    /// </summary>
    public class HtmlResourcesAggregator
    {
        protected HtmlHelper _htmlHelper;

        protected string[] _librariesNames = { "jquery", "jquery-ui", "jquery.unobtrusive-ajax" };

        public enum Libraries : int
        {
            jQuery = 0,
            jQueryUI,
            UnobtrusiveMvc
        }

        public enum Cdns
        {
            Google, Microsoft
        }

        public enum ScriptFormats
        {
            Min, Debug
        }

        protected List<string> JsFiles
        {
            get
            {
                List<string> jss = _htmlHelper.ViewData["__HtmlResourcesAggregator.Instance.Js"] as List<string>;
                if(jss == null)
                {
                    jss = new List<string>();
                    _htmlHelper.ViewData["__HtmlResourcesAggregator.Instance.Js"] = jss;
                }
                return jss;
            }
        }

        protected List<string> CssFiles
        {
            get
            {
                List<string> csss = _htmlHelper.ViewData["__HtmlResourcesAggregator.Instance.Css"] as List<string>;
                if(csss == null)
                {
                    csss = new List<string>();
                    _htmlHelper.ViewData["__HtmlResourcesAggregator.Instance.Css"] = csss;
                }
                return csss;
            }
        }

        public HtmlResourcesAggregator(HtmlHelper helper)
        {
            // Store it to get access to HTML helper
            _htmlHelper = helper;
        }

        /// <summary>
        /// Adds CSS file to be added in layout
        /// </summary>
        /// <param name="file">Full path or relative "~/"</param>
        public HtmlResourcesAggregator AddCssFile(string file)
        {
            string fullPath = file.StartsWith("~/") ? VirtualPathUtility.ToAbsolute(file) : file;
            CssFiles.Add(fullPath);
            return this;
        }

        /// <summary>
        /// Adds JS file to be added in layout
        /// </summary>
        /// <param name="file">Full path or relative "~/"</param>
        public HtmlResourcesAggregator AddJsFile(string file)
        {
            string fullPath = file.StartsWith("~/") ? VirtualPathUtility.ToAbsolute(file) : file;
            JsFiles.Add(fullPath);
            return this;
        }

        public HtmlResourcesAggregator AddJsLibrary(Libraries library, string version, ScriptFormats f, Cdns cdn)
        {
            string jsFilename = _librariesNames[(int)library];
            jsFilename = f == ScriptFormats.Min ? string.Concat(jsFilename, ".min.js") : string.Concat(jsFilename, ".js");

            if( library == Libraries.UnobtrusiveMvc )
            {
                if(cdn != Cdns.Microsoft)
                {
                    throw new ArgumentException("Google CDN does not host Microsoft Unobtrusive Ajax!");
                }
                this.JsFiles.Add(string.Concat("//ajax.aspnetcdn.com/ajax/mvc/3.0/", jsFilename));
                return this;
            }
            string baseUrl = cdn == Cdns.Google ? "//ajax.googleapis.com/ajax/libs/" : "//ajax.aspnetcdn.com/ajax/";
            this.JsFiles.Add(string.Format("{0}{1}", baseUrl, jsFilename));
            return this;
        }

        /// <summary>
        /// Returns all CSS includes as HTML string
        /// </summary>
        public MvcHtmlString OutputCssFiles()
        {
            StringBuilder sb = new StringBuilder();
            foreach( var file in CssFiles )
            {
                sb.AppendFormat("<link rel=\"stylesheet\" type=\"text/css\" href=\"{0}\">", file).AppendLine();
            }
            return MvcHtmlString.Create(sb.ToString());
        }

        /// <summary>
        /// Returns all JS includes as HTML string
        /// </summary>
        public MvcHtmlString OutputJsFiles()
        {
            StringBuilder sb = new StringBuilder();
            foreach(var file in JsFiles)
            {
                sb.AppendFormat("<script src=\"{0}\"></script>", file).AppendLine();
            }
            return MvcHtmlString.Create(sb.ToString());
        }
    }
}
