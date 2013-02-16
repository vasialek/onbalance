var gTable;
var gDataSource;
var gResultFields = [];
var arDetails = ["35", "36", "37", "38", "39", "40", "41", "42", "42.5", "43", "44", "44.5", "45", "46", "46.5", "47", "48", "49", "49.5", "50", "51", "52", "53", "54", "54.5", "55"];
var arDetailsChildren = ["122", "128", "134", "140", "152", "158", "164", "170", "176"];
var gColumnsDefinitions = [];

YAHOO.OnBalance = {
    uid: "",
    currentPosId: 100002,
    currentCategoryId: 0,
    baseUrl: "http://www.online-balance.com/",
//    baseUrl: "http://localhost:52293/",
    uiElements: {
        // Displays current path GJ->POS1
        lblPath: null,
        // Array with possible cell formats
        cellStateStyles: [
            { name: "Good", cssClass: "bg-darkgreen", id: "stateGood" },
            { name: "Bad", cssClass: "bg-red", id: "stateBad" },
            { name: "Poor", cssClass: "bg-orangered", id: "statePoor" },
            { name: "Important", cssClass: "bg-darkorange", id: "stateImportant" }
        ]
    },
    selectedCell: null,
    localChanges: [],
    availableCellColors: [],
    products: [],
    newProduct: {
        name: "",
        code: "",
        price_minor: 0
        // Sizes
//        35: 0, 36: 0, 37: 0, 38: 0, 39: 0, 40: 0, 41: 0, 42: 0, 42.5: 0, 43: 0, 44: 0, 44.5: 0, 45: 0, 46: 0, 46.5: 0, 47: 0, 48: 0, 49: 0, 49.5: 0, 50: 0, 51: 0, 52: 0, 53: 0, 54: 0, 54.5: 0, 55: 0
    },
    organizations: [
        {
            Name: "GJ Eshop",
            Categories: [
                { name: "Avalyne", id: 1001, sizes: ["33","34","35","35,5","36","36,5","37","37,5","38","38,5","39","40","41","42","42.5","43","44","44.5","45","45,5","46","46.5","47","47,5","48","48,5","49","49.5","50","50,5","51","52","52,5","53","54"] },
                { name: "Apranga vyrams ir apranga moteris", id: 1002, sizes: ["XXS"," XS"," S"," M"," L"," XL"," XXL"," XXXL"] },
                { name: "Apranga vaikams", id: 1003, sizes: ["122cm","128","134","140","152","158","164","170","176"] }
//                { name: "Kepures (priedai)", id: 1004, sizes:  }
            ]
        }
    ],
    // Menu object
    oApplicationMenu: null,
    // Menu for global tasks
    applicationMenu: [
        {
            text: "Shops",
            submenu: {
                id: "MenuShops",
                itemdata: [
                    "GJ E-shop",
                    "GJ Gariunai",
                    "GJ London"
                ]
            }
        },
        {
            text: "Categories",
            submenu: { id: "MenuCategories", itemdata: []}
        },
        {
            text: "Actions",
            submenu: {
                id: "MenuActions",
                itemdata: [
                    {
                        text: "Approve changes...",
                        onclick: {
                            fn: function(){
                                console.log("Approve changes is clicked");
                                displayPendingChangesDialog(null);
                            }
                        }
                    },
                    {
                        text: "Refresh",
                        onclick: {
                            fn: function(){
                                console.log("Refresh is clicked");
                                loadDataToTable(YAHOO.OnBalance.currentPosId, YAHOO.OnBalance.currentCategoryId);
                            }
                        }
                    }
                ]
            }
        }
    ],
    contextMenu: []
};

/**
 * Entry point
 */
function onPageLoaded()
{
    loadMainSchema(function(){
        initializeBalanceGrid();
    });
}

function isAuthorized()
{
    return (typeof YAHOO.OnBalance.uid === null) || (YAHOO.OnBalance.uid.length > 0);
}

function securePage()
{
    if( !isAuthorized() )
    {
        console.log("Login, please!");
    }
}

function createToolbar()
{
    console.log("Creating toolbar:");
    YAHOO.OnBalance.uiElements.cellStateStyles.forEach(function(style, index){
        console.log(style);
        var btn = new YAHOO.widget.Button({
            container: "idToolbar",
            label: style.name,
            title: style.name
        }).on("click", function(){
                console.log("Making cell formatted:" + style.cssClass);
                if( YAHOO.OnBalance.selectedCell != null )
                {
                    YAHOO.OnBalance.uiElements.cellStateStyles.forEach(function(s, i){
                        YAHOO.util.Dom.removeClass(YAHOO.OnBalance.selectedCell, s.cssClass);
                    });
                    YAHOO.util.Dom.removeClass(YAHOO.OnBalance.selectedCell, "dt-selected");
                    YAHOO.util.Dom.addClass(YAHOO.OnBalance.selectedCell, style.cssClass);
                }
            });
    });
}

function displayCurrentPath()
{
    console.log("Dislaying current path...");
    if( YAHOO.OnBalance.uiElements.lblPath != null )
    {
        var c = getCategoryById(YAHOO.OnBalance.currentCategoryId);
        YAHOO.OnBalance.uiElements.lblPath.innerText = c == null ? "GJ" : c.name;
    }
}

/**
 * Returns array of sizes for specified category
 * @param categoryId int
 * @return Array
 */
function getDetailsForCategory(categoryId)
{
    var category = getCategoryById(categoryId);
    return category == null ? [] : category.sizes;
}

/**
 * Returns category schema by ID or null
 * @param categoryId int
 * @return {*}
 */
function getCategoryById(categoryId)
{
    for(var i = 0; i < YAHOO.OnBalance.organizations[0].Categories.length; i++)
    {
        if( YAHOO.OnBalance.organizations[0].Categories[i].id == categoryId )
        {
            return YAHOO.OnBalance.organizations[0].Categories[i];
        }
    }

    return null;
}

function createTable(categoryId)
{
    console.log("Creating table for POS #" + categoryId);
    arColumnsDefinitions = [
        { key: "name", label: "Name", sortable: true, editor: new YAHOO.widget.TextboxCellEditor({ disableBtns: true }) },
        { key: "price_minor", label: "Price", sortable: true, editor: new YAHOO.widget.TextboxCellEditor(/*{ validator: YAHOO.widget.DataTable.validateNumber }*/) },
        { key: "code", label: "Code", sortable: true, editor: new YAHOO.widget.TextboxCellEditor({ disableBtns: true }) }
    ];
    var details = getDetailsForCategory(categoryId);
    for(var i = 0; i < details.length; i++)
    {
        gResultFields[gResultFields.length] = { key: arDetails[i] };
        arColumnsDefinitions[arColumnsDefinitions.length] = {
            key: details[i],
            label: details[i],
            editor: new YAHOO.widget.TextboxCellEditor({validator: YAHOO.widget.DataTable.validateNumber})
        };
    }

    oTable = new YAHOO.widget.ScrollingDataTable("MainBalanceDiv", arColumnsDefinitions, gDataSource, {
//    oTable = new YAHOO.widget.DataTable("MainBalanceDiv", arColumnsDefinitions, gDataSource, {
        initialLoad: false,
        height: "50em",
        selectionMode: "cell"
    });

    console.log("Created table:");
    console.log(oTable);

//    oTable.subscribe("cellMouseoverEvent", highlightEditableCell);

    // Show editor on double click
    oTable.subscribe("cellDblclickEvent", oTable.onEventShowCellEditor);

    // Remember selected cell on click
    oTable.subscribe("cellClickEvent", function(oArgs){
        console.log("Cell is clicked...");
        var target = YAHOO.util.Event.getTarget(oArgs);
        var column = this.getColumn(target);
        var record = this.getRecord(target);
        var cell = this.getTdEl({
            record: record,
            column: column
        });

        this.unselectAllCells();
        this.unselectAllRows();

        this.selectCell(target);
        YAHOO.OnBalance.selectedCell = cell;
//        YAHOO.util.Dom.get("DivStatus").innerHTML = column + "" + record;
    });

    var onContextMenuClick = function(eventType, oArgs, myDataTable)
    {
        // Extract which TR element triggered the context menu
        var elRow = this.contextEventTarget;
        // Index of clicked
        var task = oArgs[1];
        elRow = myDataTable.getTrEl(elRow);
        var styles = ["red", "green", "darkyellow", "cyan", "blue"];
        styles.forEach(function(el, index){
            YAHOO.util.Dom.removeClass(elRow, el);
        });
        YAHOO.util.Dom.addClass(elRow, styles[task.index]);
    };

    var styles = ["red", "green", "darkyellow", "cyan", "blue"];
    YAHOO.OnBalance.contextMenu = [];
    styles.forEach(function(el, index){
        YAHOO.OnBalance.contextMenu[YAHOO.OnBalance.contextMenu.length] = {
            text: "<span class='cm_" + el + "'>" + el + "</span>"
        };
    });
    var contextMenu = new YAHOO.widget.ContextMenu("OnBalanceContextMenu", {
        trigger: oTable.getTbodyEl()
    });
    contextMenu.addItems(YAHOO.OnBalance.contextMenu);
    contextMenu.render();
    contextMenu.clickEvent.subscribe(onContextMenuClick, oTable);

    return oTable;
}

function initializeBalanceGrid()
{
    securePage();
//    createApplicationMenu(YAHOO.OnBalance.organizations[0][0]);
    var posId = YAHOO.OnBalance.currentPosId;
    YAHOO.OnBalance.currentCategoryId = YAHOO.OnBalance.organizations[0].Categories[0].id;

    // Set usefull UI elements
    YAHOO.OnBalance.uiElements.lblPath = document.getElementById("CurrentPath");

    gDataSource = new YAHOO.util.ScriptNodeDataSource(YAHOO.OnBalance.baseUrl + "balance/get/");
    gResultFields = [
        { key: "name" },
        { key: "code" },
        { key: "price_minor" }
    ];
    gDataSource.responseSchema = {
        resultsList: "data",
        field: gResultFields
    }
    
    createToolbar();

/*
    gTable = createTable(posId);

    gTable.subscribe("cellMouseoverEvent", highlightEditableCell);
    gTable.subscribe("cellMouseoutEvent", gTable.onEventUnhighlightCell);
    gTable.subscribe("cellClickEvent", gTable.onEventShowCellEditor);
    gTable.subscribe("cellClickEvent", function(ev){
        var target = YAHOO.util.Event.getTarget(ev);
        var column = gTable.getColumn(target);
        if( (column.key == "Delete") && confirm("Do you want to delete?!") )
        {
            gTable.deleteRow(target);
        }});
    gTable.subscribe("cellUpdateEvent", function(record, column, oldData)
    {
        onProductChanged(record);
    });
*/
    preparePendingDialog();

    // Add product button
    YAHOO.util.Event.addListener("AddProductButton", "click", function ()
    {
        console.log("Adding new product for category #" + YAHOO.OnBalance.currentCategoryId);
        var record = YAHOO.widget.DataTable._cloneObject(YAHOO.OnBalance.newProduct);
        var details = getDetailsForCategory(YAHOO.OnBalance.currentCategoryId);
        for(var i = 0; i < details.length; i++)
        {
            record[details[i]] = 0;
        }
        record.row = record.row + 1;
        gTable.addRow(record);
    }, this, true);

    displayCurrentPath();
}

function preparePendingDialog()
{
    // Remove progressively enhanced content class, just before creating the module
    YAHOO.util.Dom.removeClass("PendingChangesDialog", "yui-pe-content");

    // Instantiate the Dialog
    YAHOO.OnBalance.PendingDialog = new YAHOO.widget.Dialog("PendingChangesDialog", {
        width: "30em",
        fixedcenter: true,
        visible: false,
        constraintoviewport: true,
        buttons: [
            { text:"Submit", handler: handlePendingSubmit, isDefault: true },
            { text:"Cancel", handler: handlePendingCancel }
        ]});

    // Render the Dialog
    YAHOO.OnBalance.PendingDialog.render();

    var buttons = document.getElementsByClassName("ShowRemotePending");
    YAHOO.util.Event.addListener(buttons, "click", displayPendingChangesDialog, YAHOO.OnBalance.PendingDialog, true);
    YAHOO.util.Event.addListener("hide", "click", YAHOO.OnBalance.PendingDialog.hide, YAHOO.OnBalance.PendingDialog, true);
}

function loadDataToTable(posId, categoryId)
{
    gTable = createTable(categoryId);
    gTable.load({
        request: "?posid=" + posId + "&categoryid=" + categoryId
//        callback: {
//            failure: function()
//            {
//                alert("Error loading data, please wait!");
//            }
//        }
    });
}


function onProductChanged(record)
{
    console.log("onProductChanged: ");
    console.log(record);
    var code = record.record._oData.code;
    var qnt = record.column.editor.value;
//    console.log(YAHOO.OnBalance.LocalChanges);
//    console.log("Internal code to update is: " + code);
    var indexOfProduct = getIndexByCode(code);
    console.log(record.record.oData);
	var details = [];
    // Not exists in array
    if( indexOfProduct < 0 )
    {
        indexOfProduct = YAHOO.OnBalance.localChanges.length;
    }else
	{
		details = YAHOO.OnBalance.localChanges[indexOfProduct].details;
	}
	details[details.length] = {
		pName: "size"
		, pVal: record.column.field
		, quantity: qnt
	};

    YAHOO.OnBalance.localChanges[indexOfProduct] = {
        code: code
        , name: record.record._oData.name
        , price: record.record._oData.price_minor
        , details: details

    }
    console.log("Pending changes:");
    console.log(YAHOO.OnBalance.localChanges);
}

function getIndexByCode(code)
{
    for(var i = 0; i < YAHOO.OnBalance.localChanges.length; i++)
    {
        if( YAHOO.OnBalance.localChanges[i].code == code )
        {
            return i;
        }
    }

    return -1;
}

function highlightEditableCell(oArgs)
{
    var elCell = oArgs.target;
    if(YAHOO.util.Dom.hasClass(elCell, "yui-dt-editable"))
    {
        this.highlightCell(elCell);
    }
}

// Define various event handlers for pending changes Dialog
function handlePendingSubmit()
{
    var s = "?_token=123456";
    var name;
    for(var i = 0; i < YAHOO.OnBalance.localChanges.length; i++)
    {
        name = YAHOO.OnBalance.localChanges[i].name.trim() == "" ? "UNKNOWN" : YAHOO.OnBalance.localChanges[i].name;
        s += "&updates[" + i + "][ProductName]=" + name + "&updates[" + i + "][Price]=" + YAHOO.OnBalance.localChanges[i].price;
        s += "&updates[" + i + "][InternalCode]=" + YAHOO.OnBalance.localChanges[i].code;
        console.log("Details:");
        console.log(YAHOO.OnBalance.localChanges[i].details);
        var details = YAHOO.OnBalance.localChanges[i].details;
        for(var j = 0; (details != null) && (j < details.length); j++)
        {
            s += "&updates[" + i + "][Sizes][" + details[j].pVal + "]=" + details[j].quantity;
        }
        //s += "&[" + i + "].ProductName=" + YAHOO.OnBalance.localChanges[i].name + "&[" + i + "].Price=" + YAHOO.OnBalance.localChanges[i].price;
        //s += "&[" + i + "].InternalCode=" + YAHOO.OnBalance.localChanges[i].code;
    }
    console.log("Sending updates to: " + s);

    //var dsScriptNode = new YAHOO.util.ScriptNodeDataSource("http://localhost:49630/balance/dosend/");
	var dsScriptNode = new YAHOO.util.ScriptNodeDataSource("http://gjsportland.com/index.php/lt/balance/dosend/");
    dsScriptNode.responseType = YAHOO.util.XHRDataSource.TYPE_JSON;
    dsScriptNode.connMethodPost = true;
    dsScriptNode.sendRequest(s, {
        success: function(oRequest, oParsedResponse, oPayload)
        {
            console.log("Sent data OK!");
            // Clear updated products
            YAHOO.OnBalance.localChanges = [];
            YAHOO.OnBalance.PendingDialog.hide();
        },
        failure: function()
        {
            console.log("failed to send data!");
            alert("Error sending to server!");
            YAHOO.OnBalance.PendingDialog.hide();
        }
    });
    console.log(s);
};
function handlePendingCancel()
{
    this.cancel();
};

function handlePendingSuccess(o)
{
    var response = o.responseText;
    response = response.split("<!")[0];
    document.getElementById("resp").innerHTML = response;
}

function handlePendingFailure(o)
{
    alert("Submission failed: " + o.status);
}

function formatLocalChangesForSubmit()
{
    console.log("Local changes:");
    console.log(YAHOO.OnBalance.localChanges);
    var s = "";
    for(var i = 0; i < YAHOO.OnBalance.localChanges.length; i++)
    {
        s += "<label>" + YAHOO.OnBalance.localChanges[i].name + ", " + YAHOO.OnBalance.localChanges[i].price + "</label>";
        s += "<input type='hidden' name='[" + i + "].InternalCode' value='" + YAHOO.OnBalance.localChanges[i].code + "' />";
        s += "<input type='hidden' name='[" + i + "].ProductName' value='" + YAHOO.OnBalance.localChanges[i].name + "' />";
        s += "<input type='hidden' name='[" + i + "].Price' value='" + YAHOO.OnBalance.localChanges[i].price + "' />";
    }
    return s;
}

function displayPendingChangesDialog(o)
{
    console.log("Displaying pending changes...");
    var columnDefinitions = [
        { label: "Approve", formatter: "checkbox"/*, editor: new YAHOO.widget.Checkbox()*/},
        { key: "pr", label: "Price" },
        { key: "name", label: "Name" }
    ];

    var oDsCallback = {
        success: function(oRequest, oParsedResponse, oPayload)
        {
            console.log("Got data from server...");
        },
        failure: function(oRequest, oParsedResponse, oPayload)
        {
            alert("Error getting data!");
        }
    };

    var oDataSource = new YAHOO.util.ScriptNodeDataSource("http://gjsportland.com/index.php/lt/balance/get/", oDsCallback);
//    oDataSource.responseType = YAHOO.util.XHRDataSource.TYPE_JSON;
    oDataSource.responseSchema = {
        //results: "",
        fields: ["uid", "code", "pr", "posid", "name", "sizes"]
    };

    oTable = new YAHOO.widget.DataTable("PendingChangesTable", columnDefinitions, oDataSource, {
        caption: "Pending changes",
        height: "5em",
        initialLoad: {
            request: "?_token=123456"
        }
    });

    var panel1 = new YAHOO.widget.Dialog("panel1", {
        visible: false,
        close: true,
        fixedcenter: true,
        buttons: [
            { text: "Approve", handler: function(){
                console.log("Approving pending changes...");
                this.cancel();
            } },
            { text: "Cancel", isDefault: true, handler: function(){
                console.log("Cancelling pending changes...");
                this.cancel();
            } }
        ]
    });
    panel1.render();
    panel1.show();
}

/**
 * Loads from OBS schema - POSes ant theirs categories
 */
function loadMainSchema(successCallback)
{
    var dsCallback = {
        failure: function(oRequest, oParsedResponse, oPayload)
        {
            console.log("failed to get main schema!");
        },
        success: function(oRequest, oParsedResponse, oPayload)
        {
            console.log("Parsed:");
            console.log(oParsedResponse);
            YAHOO.OnBalance.organizations[0].Name = oParsedResponse.meta.Name;
            YAHOO.OnBalance.organizations[0].Categories = [];
            for(var i = 0; i < oParsedResponse.results.length; i++)
            {
                YAHOO.OnBalance.organizations[0].Categories[i] = {
                    id: oParsedResponse.results[i].Id,
                    name: oParsedResponse.results[i].Name,
                    sizes: oParsedResponse.results[i].Sizes
                };
            }
            console.log("Prepared new organization from schema:");
            console.log(YAHOO.OnBalance.organizations[0]);

            createApplicationMenu();

            console.log("Calling success loaded page schema callback...");
            successCallback();
        }
    };
    var posId = YAHOO.OnBalance.currentPosId;
//    var ds = new YAHOO.util.ScriptNodeDataSource("http://localhost:52293/balance/getorganizationschema/" + posId);
    var ds = new YAHOO.util.ScriptNodeDataSource(YAHOO.OnBalance.baseUrl + "balance/getorganizationschema/" + posId);
    ds.responseType = YAHOO.util.XHRDataSource.TYPE_JSON;
    ds.responseSchema = {
        resultsList: "Results.Categories",
        metaFields: {
            Name: "Results.Name",
            Id: "Results.Id"
        }
    };
    ds.sendRequest("?", dsCallback);
}

function createApplicationMenu(oSchema)
{
//    console.log("Creating new appliction menu from schema:");
//    console.log(oSchema);
//    if( YAHOO.OnBalance.oApplicationMenu != null )
//    {
//        YAHOO.OnBalance.oApplicationMenu.destroy();
//    }
//
    console.log("Build schema from OBS categories:");
    console.log(YAHOO.OnBalance.organizations[0].Categories);

    YAHOO.OnBalance.applicationMenu[1].submenu.itemdata = [];
    for(var i = 0; i < YAHOO.OnBalance.organizations[0].Categories.length; i++)
    {
        YAHOO.OnBalance.applicationMenu[1].submenu.itemdata[i] = {
            text: YAHOO.OnBalance.organizations[0].Categories[i].name,
            onclick: { fn: function(){
                console.log("Clicked on category at position: " + this.index);
                YAHOO.OnBalance.currentCategoryId = YAHOO.OnBalance.organizations[0].Categories[this.index].id;
                gTable = createTable(YAHOO.OnBalance.currentCategoryId);
//                console.log(YAHOO.OnBalance);
                displayCurrentPath();
            }}
        }
    }

    YAHOO.OnBalance.oApplicationMenu = new YAHOO.widget.MenuBar("mymenubar", {
        lazyload: true,
        itemdata: YAHOO.OnBalance.applicationMenu
    });
    YAHOO.OnBalance.oApplicationMenu.render(document.body);

}

