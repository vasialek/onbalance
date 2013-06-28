/*
 * Namespace for working with prodcut category
 */
var Category = {
    _categoryId: -1,
    _currentCategoryType: -1,
    _isConsole: false,

    // Category types
    CATEGORY_TYPE_ORDINAL: 1,
    CATEGORY_TYPE_EXTENDED: 2,
    CATEGORY_TYPE_BIDIMENSIONAL: 3,
    CATEGORY_TYPE_MULTIDIMENSIONAL: 4,

    oCategoryTypeId: null,

    init: function(categoryId, currentCategoryTypeId)
    {
        this._categoryId = categoryId;
        this._currentCategoryType = currentCategoryTypeId;

        // Remember hidden field
        this.oCategoryTypeId = jQuery("#CategoryTypeId");

        this._isConsole = typeof console !== undefined;
        this._log("Category JS is initializing...");
        var self = this;
        jQuery("#CategoryTypeList a").click(function(){
            var categoryTypeId = self._extractIdFromName(jQuery(this).attr("id"), "CategoryTypeId_");
            self._log("Got category type ID: " + categoryTypeId);
            self._onCategoryTypeIdChanged(categoryTypeId);
        });
    },

    _onCategoryTypeIdChanged: function(categoryTypeId)
    {
        if( this._currentCategoryType !== categoryTypeId )
        {
            if( this._validateCategoryTypeId(categoryTypeId) )
            {
                this._log("Category type ID is chaged OK to: " + categoryTypeId);
                this._togglePrimaryClass(this._currentCategoryType, false);
                this._togglePrimaryClass(categoryTypeId, true);
                this._currentCategoryType = categoryTypeId;
                this.oCategoryTypeId.val(this._currentCategoryType);
            }
        }
    },

    _validateCategoryTypeId: function(categoryTypeId)
    {
        // TODO: validate for currently added parameters

        switch(categoryTypeId)
        {
            case this.CATEGORY_TYPE_ORDINAL:
                if( this._currentCategoryType === this.CATEGORY_TYPE_EXTENDED || this._currentCategoryType === this.CATEGORY_TYPE_MULTIDIMENSIONAL || this._currentCategoryType === this.CATEGORY_TYPE_BIDIMENSIONAL )
                {
                    this._displayError("CategoryTypeId", "Could not change current category type to be Ordinal!");
                    return false;
                }
                return true;
            case this.CATEGORY_TYPE_EXTENDED:
                if( this._currentCategoryType === this.CATEGORY_TYPE_MULTIDIMENSIONAL || this._currentCategoryType === this.CATEGORY_TYPE_BIDIMENSIONAL )
                {
                    this._displayError("CategoryTypeId", "Could not change current category type to Extended!");
                    return false;
                }
                return true;
            case this.CATEGORY_TYPE_BIDIMENSIONAL:
                if( this._currentCategoryType === this.CATEGORY_TYPE_MULTIDIMENSIONAL )
                {
                    this._displayError("CategoryTypeId", "Could not change current category type to Bidimensional!");
                    return false;
                }
                return true;
            case this.CATEGORY_TYPE_MULTIDIMENSIONAL:
                return true;
        }

        this._log("Bad category type ID: " + categoryTypeId);
        return false;
    },

    /**
     * Adds/removes CSS class for Category type button
     * @param {int} categoryTypeId ID of category type button
     * @param {boolean} isOn whether to add or remove CSS
     */
    _togglePrimaryClass: function(categoryTypeId, isOn)
    {
        if(isOn)
        {
            jQuery("#CategoryTypeId_" + categoryTypeId).addClass("btn-primary");
        }else
        {
            jQuery("#CategoryTypeId_" + categoryTypeId).removeClass("btn-primary");
        }
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
    },
    _extractIdFromName: function(name, prefix)
    {
        prefix = prefix === undefined ? "CategoryTypeId_" : prefix;
        this._log("Using prefix [" + prefix + "] to extract ID (int) from this string [" + name + "]");
        id = parseInt(name.replace(prefix, ""));
        return isNaN(id) ? -1 : id;
    }

};
