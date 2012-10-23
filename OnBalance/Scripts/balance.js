YAHOO.example.Data = {
    products: [
//        { name: "Adidas", code: "123456", price_minor: 150, amount: 5, price_release_minor: 220, colors: ["red"], fruit: ["banana", "cherry"], last_login: "4/19/2007" },
//        { name: "Nike", code: "123456", price_minor: 220, amount: 3, price_release_minor: 300, colors: ["red", "blue"], fruit: ["apple"], last_login: "2/15/2006" },
    ]
};

var arDetails = ["35", "36", "37", "38", "39", "40", "41", "42", "42.5", "43", "44", "44.5", "45", "46", "46.5", "47", "48", "49"];
var myColumnDefs = [
    { key: "name", label: "Name", sortable: true, editor: new YAHOO.widget.TextboxCellEditor({ disableBtns: true }) },
    { key: "code", label: "Code", sortable: true, editor: new YAHOO.widget.TextboxCellEditor({ disableBtns: true }) },
    { key: "price_minor", label: "Price", sortable: true, editor: new YAHOO.widget.TextboxCellEditor(/*{ validator: YAHOO.widget.DataTable.validateNumber }*/) },
    { key: "amount", label: "Total", sortable: true }
];
myColumnDefs[myColumnDefs.length] = {key: "QuantityControl", label: " ", formatter: function(elCell){
    var s = "<img src='/images/decrease.gif' title='Decrease' onclick='gIncreaseOrDecrease = -1' />";
    s += "<img src='/images/increase.gif' title='Increase' onclick='gIncreaseOrDecrease = 1' />";
    elCell.innerHTML = s;
    elCell.style.cursor = 'pointer';
}};

for(var i = 0; i < arDetails.length; i++)
{
    if(arDetails[i] == "40")
    {
        myColumnDefs[myColumnDefs.length] = { key: arDetails[i], label: arDetails[i], editor: new YAHOO.widget.TextboxCellEditor({validator: YAHOO.widget.DataTable.validateNumber}) };
    }else
    {
        myColumnDefs[myColumnDefs.length] = { key: arDetails[i], label: arDetails[i], editor: new YAHOO.widget.RadioCellEditor({ radioOptions: ["yes", "no"], disableBtns: true }) };
    }
}
myColumnDefs[myColumnDefs.length] = { key: "price_release_minor", label: "Price release", sortable: true, editor: new YAHOO.widget.TextboxCellEditor({ validator: YAHOO.widget.DataTable.validateNumber }) };
myColumnDefs[myColumnDefs.length] = {key: "Delete", label: " ", formatter: function(elCell){
    elCell.innerHTML = "<img src='/images/delete.png' title='delete row' />";
    elCell.style.cursor = 'pointer';
}};


YAHOO.util.Event.addListener(window, "load", function()
{
    YAHOO.example.InlineCellEditing = function()
    {
        var myDataSource = new YAHOO.util.DataSource(YAHOO.example.Data.products);
//        var myDataSource = new YAHOO.util.XHRDataSource("http://localhost:49630/pradmin/balance/100001?");
//        myDataSource.responseType = YAHOO.util.DataSource.TYPE_XHR;
//        myDataSource.connXhrMode = "queueRequests";
//        myDataSource.responseSchema = {
//            resultsList: "ResultSet.Result",
//            fields: ["Name", "Code"]
//        };
        var myDataTable = new YAHOO.widget.DataTable("MainBalanceDiv", myColumnDefs, myDataSource, {
            initialLoad: true
        });

        var mySuccessHandler = function()
        {
            alert("Success");
//            this.set("sortedBy", null);
            this.onDataReturnAppendRows.apply(this, arguments);
        };
        var myFailureHandler = function()
        {
//            alert("Error!");
            this.showTableMessage(YAHOO.widget.DataTable.MSG_ERROR, YAHOO.widget.DataTable.CLASS_ERROR);
            this.onDataReturnAppendRows.apply(this, arguments);
        };
        var callbackObj = {
            success: mySuccessHandler,
            failure: myFailureHandler,
            scope: myDataTable
        };

//        alert("sending...");
//        myDataSource.sendRequest("?", callbackObj);
//        alert("Request is sent...");

    // Set up editing flow
        var highlightEditableCell = function(oArgs)
        {
            var elCell = oArgs.target;
            if(YAHOO.util.Dom.hasClass(elCell, "yui-dt-editable"))
            {
                this.highlightCell(elCell);
            }
        };
        myDataTable.subscribe("cellMouseoverEvent", highlightEditableCell);
        myDataTable.subscribe("cellMouseoutEvent", myDataTable.onEventUnhighlightCell);
        myDataTable.subscribe("cellClickEvent", myDataTable.onEventShowCellEditor);
        myDataTable.subscribe("cellClickEvent", function(ev){
            var target = YAHOO.util.Event.getTarget(ev);
            var column = myDataTable.getColumn(target);
            if( (column.key == "Delete") && confirm("Do you want to delete?!") )
            {
                myDataTable.deleteRow(target);
            }});
//        myDataTable.subscribe("cellUpdateEvent", function(record, column, oldData)
//        {
//        });

        return {
            oDS: myDataSource,
            oDT: myDataTable
        };
    }();
});
