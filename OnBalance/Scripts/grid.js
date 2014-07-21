var Grid = {
    _products: [],
    _colors: [
        { color: "#02FF06", name: "2011 discounts", code: 0 },
        { color: "#DB08BA", name: "2011 autumn", code: 1 },
        { color: "#A64B4A", name: "2012 autumn", code: 2 },
        { color: "#00D4F2", name: "2012 spring-summer", code: 3 },
        { color: "#1D11B9", name: "2013 spring-summer", code: 4 },
        { color: "#FBF94E", name: "2013 autumn-winter", code: 5 },
        { color: "#874D80", name: "2014 spring", code: 6 },
    ],

    init: function () {
        this._initContextMenu();
        this._initProductActions();

    },

    onContextMenuClicked: function (e, menuIndex) {
        e.preventDefault();
        console.log("Clicked on context menu: " + menuIndex);
        //$(e.target).closest("tr").attr("style", "background: red;");
        var productId = this.getClickedProductId(e, "tr", "ListItem_");
        console.log("  product ID: " + productId);

        var color = this._resolveMenuItemColor(menuIndex);
        if (color !== null) {
            console.log("  color to set: " + color.name + " (" + color.color + ")");
            $(e.target).closest("td").attr("style", "background: " + color.color);
            console.log(JSON.stringify({Color:"#f00", BackgroundColor:"#00f", Remarks:"Test"}));
            //var product = this._getProductInGrid(productId);
            //console.log("  current product color: " + (product == null || product.color == null ? "NOT SET" : product.color.color));
            //if (product == null) {
            //    product = {
            //        id: productId,
            //        color: color
            //    };
            //    this._products[this._products.length] = product;
            //} else if (color.color === product.color.color) {
            //    this.clearProductColor(productId);
            //}

            //$("#ListItem_" + productId).attr("style", "background-color: " + product.color.color);
        } else {
            console.log("  Bad color!");
        }
    },

    clearProductColor: function (productId) {
        console.log("Clearing color of product #" + productId);
        console.log("  current style: " + $("#ListItem_" + productId).attr("style"));
        $("#ListItem_" + productId).attr("style", "background-color: #000");
        console.log("  remove BG style: " + $("#ListItem_" + productId).attr("style"));
    },

    getClickedProductId: function (e, closestSelector, idPrefix) {
        var s = $(e.target).closest(closestSelector).attr("id");
        if (s !== undefined) {
            var id = parseInt(s.replace(idPrefix, ""));
            return isNaN(id) ? -2 : id;
        }
        return -1;
    },

    changeProductQuantityBy: function (productId, dQnt) {
        console.log("Going to change quantity of product ID: " + productId + " by " + dQnt);
        if (productId < 1 || dQnt == 0) {
            return;
        }

        var qntDiv = $("#Qnt_" + productId);
        var currentQnt = parseInt(qntDiv.html());
        //qntDiv.html("<img src='/images/loader.gif' width='16' height='16' alt='Loading...' />");

        $.ajax({
            url: gBaseUrl + "pradmin/changequantity/" + productId,
            data: { dQnt: dQnt },
            success: function (data, status, xhr) {
                console.log("  current quantity of product is: " + qntDiv.html());
                if (isNaN(currentQnt) === false) {
                    qntDiv.html(currentQnt + dQnt).css({ "color": "green" });
                }
            },
            error: function (status, xhr) {
                console.log("Error changing product quantity! ID: " + productId);
            }
        });
    },

    reportError: function (msg) {
        alert(msg);
    },

    addNewSizeQuantity: function (objToUpdate, productId, sizeName) {
        if (productId < 1) {
            this.reportError("Error determing ID of product (contact administrator)");
            return;
        }
        if (sizeName == "") {
            this.reportError("Error determing size (contact administrator)");
            return;
        }

        console.log("Adding new product for size: " + sizeName);
        var self = this;
        //var qntDiv = $("#NewS_" + productId);

        $.ajax({
            url: gBaseUrl + "pradmin/donewsize/" + productId,
            type: "POST",
            data: { sname: sizeName },
            success: function (data, status, xhr) {
                console.log("  Added new product for size: " + sizeName);
                console.log(data);
                if (data.Status) {
                    console.log("    Got OK for new size");
                    objToUpdate
                        .unbind("click")
                        .html(data.HtmlData)
                        .css({ "color": "green" });
                    // Bind increase/decrese events for new size
                    $(".product-qnt-plus", objToUpdate).click(function (e) {
                        var id = self.getClickedProductId(e, "td", "Pr_");
                        self.changeProductQuantityBy(id, 1);
                    });
                    $(".product-qnt-minus", objToUpdate).click(function (e) {
                        var id = self.getClickedProductId(e, "td", "Pr_");
                        self.changeProductQuantityBy(id, -1);
                    });

                    objToUpdate.parent().attr("id", "Pr_" + data.NewSizeId);
                } else {
                    self.reportError("Server error, adding new size");
                }
            },
            error: function (status, xhr) {
                console.log("Error adding product to size: " + sizeName);
                self.reportError("Internal server error (500) adding new size");
            }
        });
    },

    _initContextMenu: function () {
        var self = this;
        var items = [];
        for (var i = 0; i < this._colors.length; i++) {
            items[items.length] = {
                _index: i,
                label: this._colors[i].name,
                icon: gBaseUrl + "images/menu/color_" + this._colors[i].code + ".gif",
                action: function (e) {
                    self.onContextMenuClicked(e, this._index);
                }
            };
        }
        $("table").contextPopup({
            title: "Product actions",
            items: items,
        });
    },

    _initProductActions: function () {
        var self = this;
        $(".product-qnt-plus").click(function (e) {
            var id = self.getClickedProductId(e, "td", "Pr_");
            self.changeProductQuantityBy(id, 1);
        });
        $(".product-qnt-minus").click(function (e) {
            var id = self.getClickedProductId(e, "td", "Pr_");
            self.changeProductQuantityBy(id, -1);
        });

        // Creation of size name
        $(".product-new-size").click(function (e) {
            console.log($(e.target));
            var sizeName = $(e.target).attr("data-size-name");
            var productId = $(e.target).attr("data-product-id");
            if (confirm("Do you want to add product for size: " + sizeName + "?")) {
                //var productId = self.getClickedProductId(e, "td", "NewS_");
                self.addNewSizeQuantity($(e.target), productId, sizeName);
            }
        });

        $(".product-add-new").click(function (e) {
            e.preventDefault();
            console.log($(e.target));
            var oLink = $(e.target).parent();
            var categoryId = oLink.attr("data-category-id");
            var totalSize = oLink.attr("data-size-qnt");
            console.log("Category to add new product is: " + categoryId);
            console.log("  there are total sizes: " + totalSize);
            if (confirm("Do you want to add new product?")) {
                self.addNewProduct(categoryId, totalSize);
            }
        });
    },

    addNewProduct: function (categoryId, totalSize) {
        $("#LoaderDiv").show();
        $("#DynamicDiv").html("");

        $.ajax({
            url: gBaseUrl + "pradmin/getnewproduct/@Model.PosId",
            data: "categoryId=" + categoryId + "&sizes=" + totalSize,
            success: function (data) {
                $("#LoaderDiv").hide();
                $("#DynamicDiv").html(data);
            },
            error: function (status) {
                $("#LoaderDiv").hide();
                alert("Error displaying new product form: " + status);
            },
        });
        var self = this;
        $("#DialogDiv").dialog({
            resizable: false,
            height: 450,
            width: 500,
            modal: true,
            buttons: {
                "Create": function () {
                    var productName = $("#ProductName").val();
                    if (productName.length < 3) {
                        alert("Product name should be at least 3");
                        return;
                    }
                    var oDialog = this;
                    $.ajax({
                        url: gBaseUrl + "pradmin/donewproduct/@Model.PosId",
                        data: $("#NewProductForm").serialize(),
                        type: "POST",
                        success: function (data) {
                            $("#ProCat_" + categoryId).append(data);
                            $(oDialog).dialog("close");
                            self._initializeCreatedProductActions(categoryId);
                        },
                        error: function (status) {
                            alert("Error creating new product: " + productName);
                        }
                    });
                },
                Cancel: function () {
                    $(this).dialog("close");
                }
            }
        });
    },

    _initializeCreatedProductActions: function (categoryId) {
        var self = this;
        $(".product-created-size.no-init").each(function (i, e) {
            console.log(e);
            // Prevent multiple initialization
            $(e).removeClass("no-init");
            $(e).click(function (sizeE) {
                var oSize = $(sizeE.target);
                var sizeIndex = oSize.attr("data-size-index");
                console.log("Size of index: " + sizeIndex);
                var productId = $(sizeE.target).closest("tr").attr("data-product-id");
                console.log("ID of product: " + productId);
                var sizeName = $("#Siz_" + categoryId + "_" + sizeIndex).attr("data-size-name");
                console.log("Name of size: " + sizeName);
                self.addNewSizeQuantity($(sizeE.target), productId, sizeName);
            });
        });
    },

    _resolveMenuItemColor: function (menuIndex) {
        for (var i = 0; i < this._colors.length; i++) {
            if (this._colors[i].code === menuIndex) {
                return this._colors[i];
            }
        }
        return null;
    },

    _getProductInGrid: function (productId) {
        for (var i = 0; i < this._products.length; i++) {
            if (this._products[i].id == productId) {
                return this._products[i];
            }
        }
        return null;
    }
};

