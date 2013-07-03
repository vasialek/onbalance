/*
 * Namespace for working with product
 */
var Product = {
    _isConsole: false,
    screenH: 0,
    screenW: 0,
    popupDiv: null,

    isAjaxLoaded: true,
    isAjaxPosted: true,

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
            var pos = self._displayProductInfo(jQuery(this).attr("href"), jQuery(this));
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

    _displayProductInfo: function(productUid, oTarget)
    {
        var self = this;
        var popup = this._prepareDivForLoading(this._getSpecDiv("PopupDiv"), oTarget);
        var mask = this._prepareMask(popup);
        jQuery.ajax({
            url: "/balance/getproductinfo/" + productUid,
            success: function(data){
                self._onAjaxSuccess(data, popup, "information");
            },
            error: function(){
                self._onAjaxError(popup, "information");
            }
        });
    },

    _onAjaxSuccess: function(data, oInfoDiv, typeOfInfo)
    {
        oInfoDiv.html(data);
        var mask = this._getSpecDiv("MaskDiv").hide();
    },

    _onAjaxError: function(oInfoDiv, typeOfInfo)
    {
        oInfoDiv.hide();
        var mask = this._getSpecDiv("MaskDiv").hide();
        this._displayError("", "Error loading " + typeOfInfo);
    },

    _prepareMask: function(divToClose)
    {
        var self = this;
        var mask = this._getSpecDiv("MaskDiv");
        mask.css({
            position: "absolute",
            left: 0,
            top: 0,
            width: this.screenW,
            height: this.screenH,
            background: "#000",
            opacity: 0.2,
            "z-index": 100
        }).click(function(){
            // TODO: clean event handler on close
            divToClose.hide();
            mask.hide();
        }).show();
        return mask;
    },

    _prepareDivForLoading: function(oDiv, oTarget)
    {
        oDiv.css({
            top: oTarget.position().top,
            left: oTarget.position().left,
            position: "absolute",
            "z-index": 110
        }).html("<img src='/images/loader.gif' width='16' height='16' alt='Loading...' />").show();
        return oDiv;
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

    _getImageToLoad: function()
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
