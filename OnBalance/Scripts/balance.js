﻿var gTable;
var gDataSource;
var gResultFields = [];
var arDetails = ["35", "36", "37", "38", "39", "40", "41", "42", "42.5", "43", "44", "44.5", "45", "46", "46.5", "47", "48", "49", "49.5", "50", "51", "52", "53", "54", "54.5", "55"];
var arDetailsChildren = ["122", "128", "134", "140", "152", "158", "164", "170", "176"];
var gColumnsDefinitions = [];

YAHOO.OnBalance = {
    uid: "",
    currentPosId: 100002,
    localChanges: [],
    availableCellColors: [],
    products: [],
    newProduct: {
        name: "",
        code: "",
        price_minor: 0,
        // Sizes
        35: 0, 36: 0, 37: 0, 38: 0, 39: 0, 40: 0, 41: 0, 42: 0, 42.5: 0, 43: 0, 44: 0, 44.5: 0, 45: 0, 46: 0, 46.5: 0, 47: 0, 48: 0, 49: 0, 49.5: 0, 50: 0, 51: 0, 52: 0, 53: 0, 54: 0, 54.5: 0, 55: 0
    },
    organizations: [
        {
            name: "GJ Eshop",
            categories: [
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
            submenu: {
                id: "MenuCategories",
                itemdata: [
                    { text: "Avalyne", onclick: { fn: function(){ console.log(YAHOO.OnBalance.organizations); CreateTableBySchema( YAHOO.OnBalance.organizations[0].categories[this.index]); } }, keylistener: { ctrl: true, keys: 49 } },
                    { text: "Apranga vyrams ir apranga moteris", onclick: { fn: function(){ CreateTableBySchema( YAHOO.OnBalance.organizations[0].categories[this.index]); } }, keylistener: { ctrl: true, keys: 50 } },
                    { text: "Apranga vaikams", onclick: { fn: function(){ CreateTableBySchema( YAHOO.OnBalance.organizations[0].categories[this.index]); } }, keylistener: { ctrl: true, keys: 51 } },
//                    { text: "Kepures (priedai)", onclick: { fn: function(){ CreateTableBySchema( YAHOO.OnBalance.organizations.categories[this.index]); } }, keylistener: { ctrl: true, keys: 52 } }
//                        "Avalyne", "Apranga vyrams ir apranga moteris", "Apranga vaikams", {text: "Kepures (priedai)", onclick: { fn: onMenuItemClick } }
                ]
            }
        }
    ],
    contextMenu: []
};

function isAuthorized()
{
    return YAHOO.OnBalance.uid.length > 0;
}

function securePage()
{
    if( !isAuthorized() )
    {
        console.log("Login, please!");
    }
}

function CreateTableBySchema(oSchema)
{
    console.log("Creating table by schema...");
    var columnDefinitions = [
        { key: "name", label: "Name", sortable: true, editor: new YAHOO.widget.TextboxCellEditor({ disableBtns: true }) },
        { key: "price_minor", label: "Price", sortable: true, editor: new YAHOO.widget.TextboxCellEditor(/*{ validator: YAHOO.widget.DataTable.validateNumber }*/) },
        { key: "code", label: "Code", sortable: true, editor: new YAHOO.widget.TextboxCellEditor({ disableBtns: true }) }
    ];
    for(var i = 0; i < oSchema.sizes.length; i++)
    {
//        gResultFields[gResultFields.length] = { key: arDetails[i] };
        columnDefinitions[columnDefinitions.length] = { key: oSchema.sizes[i], label: oSchema.sizes[i], editor: new YAHOO.widget.TextboxCellEditor({validator: YAHOO.widget.DataTable.validateNumber}) };
    }

    oTable = new YAHOO.widget.ScrollingDataTable("MainBalanceDiv", columnDefinitions, gDataSource, {
        initialLoad: false,
        height: "50em"
    });

    return oTable;
}

function CreateTable(categoryId)
{
    console.log("Creating table for POS #" + categoryId);
    gColumnsDefinitions = [
        { key: "name", label: "Name", sortable: true, editor: new YAHOO.widget.TextboxCellEditor({ disableBtns: true }) },
        { key: "price_minor", label: "Price", sortable: true, editor: new YAHOO.widget.TextboxCellEditor(/*{ validator: YAHOO.widget.DataTable.validateNumber }*/) },
        { key: "code", label: "Code", sortable: true, editor: new YAHOO.widget.TextboxCellEditor({ disableBtns: true }) }
    ];
    var details = [];
    switch(categoryId)
    {
        case 100001:
            details = ["35", "36", "37", "38", "39", "40", "41", "42", "42.5", "43", "44", "44.5", "45", "46", "46.5", "47", "48", "49", "49.5", "50", "51", "52", "53", "54", "54.5", "55"];
            break;
        case 100002:
            details = ["122", "128", "134", "140", "152", "158", "164", "170", "176"];
            break;
    }
    for(var i = 0; i < details.length; i++)
    {
        gResultFields[gResultFields.length] = { key: arDetails[i] };
        gColumnsDefinitions[gColumnsDefinitions.length] = { key: details[i], label: details[i], editor: new YAHOO.widget.TextboxCellEditor({validator: YAHOO.widget.DataTable.validateNumber}) };
    }

    oTable = new YAHOO.widget.ScrollingDataTable("MainBalanceDiv", gColumnsDefinitions, gDataSource, {
        initialLoad: false,
        height: "50em"
    });

    return oTable;
}

function InitializeBalanceGrid()
{
    securePage();
    loadMainSchema();
//    YAHOO.namespace("OnBalance");
    var posId = YAHOO.OnBalance.currentPosId;
    gDataSource = new YAHOO.util.ScriptNodeDataSource("http://online-balance.com/pradmin/get/");
    gResultFields = [
        { key: "name" },
        { key: "code" },
        { key: "price_minor" }
    ];
    gDataSource.responseSchema = {
        resultsList: "data",
        field: gResultFields
    }
    gTable = CreateTable(posId);
/*
    gTable = new YAHOO.widget.ScrollingDataTable("MainBalanceDiv", gColumnsDefinitions, gDataSource, {
        initialLoad: false,
        height: "50em"
    });
*/
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
    gTable.on("initEvent",function()
    {
//        gTable.setAttributes({width: "100%"},true);
//        alert("100%");
    });

    var onContextMenuClick = function(eventType, oArgs, myDataTable)
    {
        // Extract which TR element triggered the context menu
        var elRow = this.contextEventTarget;
        // Index of clicked
        var task = oArgs[1];
        console.log("Task:");
        console.log(task);
        elRow = myDataTable.getTrEl(elRow);
        console.log("El row:");
        console.log(elRow);
        var styles = ["red", "green", "darkyellow", "cyan", "blue"];
        styles.forEach(function(el, index){
            YAHOO.util.Dom.removeClass(elRow, el);
        });
        YAHOO.util.Dom.addClass(elRow, styles[task.index]);
    };

    var styles = ["red", "green", "darkyellow", "cyan", "blue"];
    styles.forEach(function(el, index){
        YAHOO.OnBalance.contextMenu[YAHOO.OnBalance.contextMenu.length] = {
            text: "<span class='cm_" + el + "'>" + el + "</span>"
        };
    });
    var contextMenu = new YAHOO.widget.ContextMenu("OnBalanceContextMenu", {
        trigger: gTable.getTbodyEl()
    });
    contextMenu.addItems(YAHOO.OnBalance.contextMenu);
    contextMenu.render();
    contextMenu.clickEvent.subscribe(onContextMenuClick, gTable);

//    loadDataToTable(posId);

    preparePendingDialog();

    // Add product button
    YAHOO.util.Event.addListener("AddProductButton", "click",function ()
    {
        console.log("Adding new product...");
        var record = YAHOO.widget.DataTable._cloneObject(YAHOO.OnBalance.newProduct);
        record.row = record.row + 1;
        gTable.addRow(record);
    },this, true);
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

function loadDataToTable(posId)
{
    gTable = CreateTable(posId);
    gTable.load({
        request: "?posid=" + posId
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
    var dlg = YAHOO.OnBalance.PendingDialog;
    var s = YAHOO.OnBalance.localChanges.length > 0 ? "" : "@OnBalance.MyMessages.Balancer.NoPendingLocalChanges";
    for( var i = 0; i < YAHOO.OnBalance.localChanges.length; i++ )
    {
        s += "<label>" + YAHOO.OnBalance.localChanges[i].name + ", " + YAHOO.OnBalance.localChanges[i].price + "</label>";
    }
    document.getElementById("PendingChangesDiv").innerHTML = formatLocalChangesForSubmit();
    dlg.show();
}

/**
 * Loads from OBS schema - POSes ant theirs categories
 */
function loadMainSchema()
{
    var posId = YAHOO.OnBalance.currentPosId;
    var ds = new YAHOO.util.ScriptNodeDataSource("http://localhost:52293/balance/getorganizationstructure/" + posId);
    ds.responseType = YAHOO.util.XHRDataSource.TYPE_JSON;
//    ds.connMethodPost = true;
    ds.sendRequest("?", {
        success: function(oRequest, oParsedResponse, oPayload)
        {
            console.log("Got main schema:");
            console.log(oParsedResponse);
            createApplicationMenu(oParsedResponse[0]);
            // Clear updated products
        },
        failure: function()
        {
            console.log("failed to get main schema!");
        }
    });
}

function createApplicationMenu(oSchema)
{
    console.log("Creating new appliction menu...");
    if( YAHOO.OnBalance.oApplicationMenu != null )
    {
        YAHOO.OnBalance.oApplicationMenu.destroy();
    }

    YAHOO.OnBalance.applicationMenu.organizations[0] = oSchema;

    YAHOO.OnBalance.oApplicationMenu = new YAHOO.widget.MenuBar("mymenubar", {
        lazyload: true,
        itemdata: YAHOO.OnBalance.applicationMenu
    });
    YAHOO.OnBalance.oApplicationMenu.render(document.body);

}

