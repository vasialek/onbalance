(function(){
    OnBalance.mainUi = {
        baseUri: "file:///c:/_projects/onbalance/OnBalance/"
    };

    var loader = new YAHOO.util.YUILoader({
        base: "http://yui.yahooapis.com/2.9.0/build/",
        require: ["reset-fonts-grids", "utilities", "logger", "button", "container", "tabview", "selector", "resize", "layout", "menu", "treeview"],
        rollup: true,

        onSuccess: function()
        {
            var base = "../onbalance/OnBalance/";
            YAHOO.log("Loaded main JS file...", "info", "main.js");
            YAHOO.util.Get.css(base + "css/balanceui.css");

            YAHOO.log("Create the first layout on the page", "info", "main.js");
            OnBalance.mainUi.layout = new YAHOO.widget.Layout({
                minWidth: 1000,
                units: [
                    { position: "top", height: 45, resize: false, body: "top1" },
                    { position: "right", width: 300, body: "<div id='Logger'></div>", header: "Logger", collapse: true },
                    { position: "left", width: 190, resize: true, body: "left1", header: "Categories", gutter: "0 5 0 5px", minWidth: 150 },
                    { position: "center", gutter: "0 5px 0 2" }
                ]
            });

            //On resize, resize the left and right column content
            OnBalance.mainUi.layout.on('resize', function() {
                var l = this.getUnitByPosition('left');
                var th = l.get('height') - YAHOO.util.Dom.get('folder_top').offsetHeight;
                var h = th - 4; //Borders around the 2 areas
                h = h - 9; //Padding between the 2 parts
                YAHOO.util.Dom.setStyle('folder_list', 'height', h + 'px');

            }, OnBalance.mainUi.layout, true);

            //On render, load tabview.js and button.js
            OnBalance.mainUi.layout.on("render", function() {
                new YAHOO.widget.LogReader("Logger");
                window.setTimeout(function() {
                    YAHOO.util.Get.script(base + "scripts/balancetabs.js");
                    //YAHOO.util.Get.script(base + "assets/js/buttons.js");
                }, 0);
                OnBalance.mainUi.layout.getUnitByPosition("right").collapse();
                setTimeout(function() {
                    YAHOO.util.Dom.setStyle(document.body, "visibility", "visible");
                    OnBalance.mainUi.layout.resize();
                }, 1000);

                var items = [
                    { text: "GJ sportland", value: 1 },
                    { text: "Nerpigiau", value: 2 },
                    { text: "El-turgus", value: 3 }
                ];
                var oButton = new YAHOO.widget.Button("OrganizationMenu", {
                    type: "menu",
                    menu: items,
                    label: "GJ"
                });
                oButton.getMenu().cfg.config.clicktohide.value = false;

//                var categoryTree = new YAHOO.widget.TreeView("CategoryList", [
//                    {label: "GJ sportland", hasIcon: false, expanded: true, type: "text", name: "500015", children: []},
////                        {label: "Men's shoes", name: "500020"},
////                        {label: "Women's shoes", name: "500020"},
////                        {label: "Children's shoes", name: "500020"}
//                    {label: "GJ gariunai", expanded: true, name: "500016", children: [] }
//                ]);

                var categoryTree = new YAHOO.widget.TreeView("CategoryList", [
                    {label: "GJ sprotland.com", type: "text", expanded: true, hasIcon: false, children: [
                        {label: "Men's shoes", name: "500020"},
                        {label: "Women's shoes", name: "500020"},
                        {label: "Children's shoes", name: "500020"}
                    ]},
                    {label: "GJ gariunai", type: "text", hasIcon: false}
                ]);
                categoryTree.render();
            });
            //Render the layout
            OnBalance.mainUi.layout.render();
        }
    });
    loader.insert();

})();
