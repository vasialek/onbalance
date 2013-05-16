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
            if( (library == Libraries.UnobtrusiveMvc) && (cdn != Cdns.Microsoft) )
            {
                throw new ArgumentException("Reuired CDN (" + cdn + ") does not host Microsoft Unobtrusive Ajax!");
            }

            //version = version.StartsWith("-") ? version : string.Concat("-", version);
            if(version.EndsWith("."))
            {
                version = version.Substring(0, version.Length - 1);
            }

            string jsFilename = _librariesNames[(int)library];
            string baseUrl = "";// = cdn == Cdns.Google ? "//ajax.googleapis.com/ajax/libs/" : "//ajax.aspnetcdn.com/ajax/";
            string urlFmt = "";
            switch(cdn)
            {
                case Cdns.Google:
                    // //ajax.googleapis.com/ajax/libs/jquery/2.0.0/jquery.min.js
                    switch(library)
                    {
                        case Libraries.jQuery:
                            urlFmt = "//ajax.googleapis.com/ajax/libs/%LIBRARY_SHORT_NAME%/%LIBRARY_VERSION%/%LIBRARY_SHORT_NAME%%FILE_FORMAT%.js";
                            break;
                        default:
                            throw new NotImplementedException("Google CDN is not supported yet!!!");
                    }
                    break;
                case Cdns.Microsoft:
                    // //ajax.aspnetcdn.com/ajax/jquery/jquery-1.9.0.js

                    switch (library)
	                {
                        case Libraries.jQuery:
                            baseUrl = "//ajax.aspnetcdn.com/ajax/jquery/";
                            //urlFmt = "//ajax.aspnetcdn.com/ajax/%LIBRARY_SHORT_NAME%/%LIBRARY_SHORT_NAME%-%LIBRARY_VERSION%.js";
                            break;
                        case Libraries.jQueryUI:
                            baseUrl = "//ajax.aspnetcdn.com/ajax/jquery.ui/";
                            break;
                        case Libraries.UnobtrusiveMvc:
                            baseUrl = "//ajax.aspnetcdn.com/ajax/mvc/3.0/";
                            break;
	                }

                    break;
            }
            if(string.IsNullOrEmpty(urlFmt))
            {
                jsFilename = f == ScriptFormats.Min ? string.Concat(jsFilename, version, ".min.js") : string.Concat(jsFilename, version, ".js");
            } else
            {
                jsFilename = urlFmt
                    .Replace("%LIBRARY_SHORT_NAME%", "jquery")
                    .Replace("%LIBRARY_VERSION%", version)
                    .Replace("%FILE_FORMAT%", f == ScriptFormats.Min ? ".min" : "");
            }

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
