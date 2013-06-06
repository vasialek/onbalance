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
            jQuery(this).popover({
                title: "Xxx Bbb",
                content: function(){
                    setTimeout(function(){return "xxx zzz";}, 5000);
                }
            });
            return false;
        });
        jQuery(".ob-prphotos-link").click(function(e){
//            self._log("Displaying product photos...");
//            self._log(e);
//            self._log(this);
//            self._log("Going to open link: " + jQuery(this).attr("href"));
            self._displayProductPhotos(jQuery(this).attr("href"));
            e.preventDefault();
            return false;
        });
    },

    _displayProductPhotos: function(href)
    {
        this._log("Displaying photos popup...");
        var mask = this._getSpecDiv("MaskDiv");

        var popup = this._getSpecDiv("PopupDiv");
        popup.html("<img src='/images/loader.gif' width='16' height='16' alt='Loading...' />");
        popup.css({"border": "0", "width": "300px", "height": "400px"});
        popup.show();

        var self = this;
        var img = new Image();
        this._log("Setting image to load SRC attribute to: " + href);
        img.src = href;
        img.onload = function()
        {
            self._log("Image is loaded, put it into DIV");
            popup.html(img);
            img.className ="img-polaroid";
            popup.width(jQuery(img).width() + 10).height(jQuery(img).height() + 10);
        }
//        this._log("SRC of ImageToLoad: " + img.attr("src"));
//        img.load();
//        img.one("load", function(){
//            self._log("Image is loaded...");
//        }).each(function(){
//            self._log("is complete?");
//            if(this.complete)
//            {
//                jQuery(this).load();
//                popup.html(img);
//            }
//        });
    },

    _getImgageToLoad: function()
    {
        var img = jQuery("#ImageToLoad");
        if( img.length < 1 )
        {
            img = jQuery("body").append("<img id='ImageToLoad' src=''>");
        }
        img.attr("src", "");
        return img;
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
