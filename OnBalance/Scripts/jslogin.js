var JsLogin = {
    oLogin: function()
    {
        var dlg = new YAHOO.widget.Dialog("LoginDiv", {
            width: "300px",
            visible : false,
            fixedcenter: true,
            constraintoviewport : true,
            close: false,
            buttons: [
                {
                    text: "Login",
                    isDefault: true,
                    handler: function()
                    {
                        var data = this.getData();
                        if( (data.username.length < 2) || (data.password.length < 1) )
                        {
                            alert("Please enter username and password!");
                            this.focusFirst();
                            return false;
                        }

                        var sUrl = YAHOO.OnBalance.baseUrl + "user/dologin?username=" + data.username + "&password=" + data.password;
                        var loader = new YAHOO.util.ScriptNodeDataSource(sUrl);
                        loader.responseType = YAHOO.util.ScriptNodeDataSource.TYPE_SCRIPTNODE;
                        loader.responseSchema = {
                            fields: [
                                { key: "Status" }, { key: "Token" }, { key: "ClientIp" }
                            ]
                        }

                        loader.sendRequest("", {
                                success: function(oRequest, oParsedResponse)
                                {
                                    var dataToCheck = data.username + "_" + data.password + "_" + oParsedResponse.results.ClientIp;
                                    console.log("Hash from server: " + oParsedResponse.results.Hash);
                                    var dataMd5 = md5(dataToCheck);
                                    console.log("MD5 to check:     " + dataMd5);
                                    if( dataMd5 == oParsedResponse.results.Hash )
                                    {
                                        authorizeUser(oParsedResponse.results.Hash, oParsedResponse.results.ClientIp);
                                        dlg.hide();
                                    }
                                },
                                failure: function()
                                {
                                    alert("Error logging in!");
                                }
                            }
                        );
                    }}
            ]
        });

        dlg.render();

        return dlg;
    }
}
