/*
 * Namespace for working with product
 */
var Product = {
    _isConsole: false,

    init: function()
    {
        this._isConsole = typeof console !== undefined;
        this._log("Product JS is initializing...");
        var self = this;
        jQuery(".ob-prinfo-link").click(function(e){
            self._log("Displaying product info...");
            e.preventDefault();
            return false;
        });
        jQuery(".ob-prphotos-link").click(function(e){
            self._log("Displaying product photos...");
            self._log(e);
            self._log(this);
            self._log("Going to open link: " + jQuery(this).attr("href"));
            e.preventDefault();
            return false;
        });
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
