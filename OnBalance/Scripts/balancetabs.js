(function() {
    var Dom = YAHOO.util.Dom,
        Event = YAHOO.util.Event,
        Sel = YAHOO.util.Selector;
        YAHOO.log("balancetabs.js loaded", "info", "balancetabs.js");

        //Method to Resize the tabview
        OnBalance.mainUi.resizeTabView = function() {
            var ul = OnBalance.mainUi.tabView._tabParent.offsetHeight;
            Dom.setStyle(OnBalance.mainUi.tabView._contentParent, "height", ((OnBalance.mainUi.layout.getSizes().center.h - ul) - 2) + "px");
        };
        
        //Listen for the layout resize and call the method
        OnBalance.mainUi.layout.on("resize", OnBalance.mainUi.resizeTabView);
        //Create the tabView
        YAHOO.log("Creating the main TabView instance", "info", "balancetabs.js");
        OnBalance.mainUi.tabView = new YAHOO.widget.TabView();
        //Create the Home tab       
        OnBalance.mainUi.tabView.addTab( new YAHOO.widget.Tab({
            //Inject a span for the icon
            label: "<span></span>Balance",
            id: "balanceView",
            content: "",
            active: true
        }));
        //Create the Remote pending tab
        OnBalance.mainUi.tabView.addTab( new YAHOO.widget.Tab({
            //Inject a span for the icon
            label: "<span></span>Remote changes",
            id: "remoteChangesView",
            content: ""

        }));
        //Create the Local pending tab
        OnBalance.mainUi.tabView.addTab( new YAHOO.widget.Tab({
            //Inject a span for the icon
            label: "<span></span>Local changes",
            id: "localChangesView",
            content: ""

        }));
        OnBalance.mainUi.tabView.on("activeTabChange", function(ev) {
            //Tabs have changed
            if (ev.newValue.get("id") == "inboxView") {
                //inbox tab was selected
                if (!OnBalance.mainUi.inboxLoaded && !OnBalance.mainUi.inboxLoading) {
                    YAHOO.log("Fetching the inbox.js file..", "info", "balancetabs.js");
                    YAHOO.log("Inbox is not loaded yet, use Get to fetch it", "info", "balancetabs.js");
                    YAHOO.log("Adding loading class to tabview", "info", "balancetabs.js");
                    OnBalance.mainUi.getFeed();
                }
            }
            //Is an editor present?
            if (OnBalance.mainUi.editor) {
                if (ev.newValue.get("id") == "composeView") {
                    YAHOO.log("Showing the ediitor", "info", "balancetabs.js");
                    OnBalance.mainUi.editor.show();
                    OnBalance.mainUi.editor.set("disabled", false);
                } else {
                    YAHOO.log("Hiding the editor", "info", "balancetabs.js");
                    OnBalance.mainUi.editor.hide();
                    OnBalance.mainUi.editor.set("disabled", true);
                }
            }
            //Resize to fit the new content
            OnBalance.mainUi.layout.resize();
        });
        //Add the tabview to the center unit of the main layout
        var el = OnBalance.mainUi.layout.getUnitByPosition("center").get("wrap");
        OnBalance.mainUi.tabView.appendTo(el);

        //resize the TabView
        OnBalance.mainUi.resizeTabView();

        
//        YAHOO.log("Fetch the news feed", "info", "balancetabs.js");
//        YAHOO.util.Get.script("assets/js/news.js");


        //When inboxView is available, update the height..
        Event.onAvailable("inboxView", function() {
            var t = OnBalance.mainUi.tabView.get("tabs");
            for (var i = 0; i < t.length; i++) {
                if (t[i].get("id") == "inboxView") {
                    var el = t[i].get("contentEl");
                    el.id = "inboxHolder";
                    YAHOO.log("Setting the height of the TabViews content parent", "info", "balancetabs.js");
                    Dom.setStyle(el, "height", Dom.getStyle(OnBalance.mainUi.tabView._contentParent, "height"));
                    
                }
            }

        });

})();
