/*
 * Namespace for working with product
 */
var Product = {
    _isConsole: false,
    screenH: 0,
    screenW: 0,

    init: function()
    {
        this._isConsole = typeof console !== undefined;
        this._log("Product JS is initializing...");

        this.screenH = jQuery(window).height();
        this.screenW = jQuery(window).width();
        this._log("Window size is: [" + this.screenW + "x" + this.screenH + "]");

        var self = this;
        jQuery(".ob-prinfo-link").click(function(e){
            self._log("Displaying product info...");
            e.preventDefault();
            return false;
        });
        jQuery(".ob-prphotos-link").click(function(e){
//            self._log("Displaying product photos...");
//            self._log(e);
//            self._log(this);
//            self._log("Going to open link: " + jQuery(this).attr("href"));
            self._displayProductPhotos();
            e.preventDefault();
            return false;
        });
    },

    _displayProductPhotos: function()
    {
        this._log("Displaying photos popup...");
        var mask = this._getSpecDiv("MaskDiv");

        var popup = this._getSpecDiv("PopupDiv");
        popup.html("Loading...");
        popup.css({"border": "solid 1px red", "width": "300px", "height": "400px"});
        popup.show();
    },

    _getSpecDiv: function(id)
    {
        var idName = "#" + id;
        var className = id.toLowerCase();
        var p = jQuery(idName);
        if(p.length < 1)
        {
            jQuery("body").append("<div id='" + id + "' class='div-" + className + "'>...</div>");
            p = jQuery(idName);
        }
        return p;
    },

    _displayError: function(field, msg)
    {
        alert("Field `" + field + "`: " + msg);
    },

    _log: function(msg)
    {
        if(this._isConsole)
        {
            console.log(msg);
        }
    }
};
