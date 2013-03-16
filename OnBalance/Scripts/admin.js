/**
 * User: Vasinov Aleksej
 * Date: 13.3.16
 */

var gBaseUrl = "/";
var gIsDataPosted = true;
var gIsConsole = typeof console == "object";

var gStatuses = {
    Approved: 1,
    Deleted: 2,
    Pending: 3,
    Completed: 4,
    Unknown: 5,
    Failed: 6
}

var gI18nMessages = {
    AjaxRequestIsBusy: "Request is not finished, please try later!",
    AjaxRequestFailed: "Error performing Ajax request, call moderator!"
}

function InitializeRolesAdmin(username)
{
    var v = false;
    jQuery("input[type=checkbox][name^='role_']").change(function(e)
    {
        console_log("User clicked on role: " + jQuery(this).attr("name"));
        var rolename = jQuery(this).attr("name");
        rolename = rolename.replace("role_", "");
        if(jQuery(this).is(":checked"))
        {
            v = false;
            if(toggleUserRole(username, rolename, true))
            {
                jQuery(this).attr("checked", true);
            }
        } else
        {
            v = true;
            if( toggleUserRole(username, rolename, false) )
            {
                jQuery(this).attr("checked", false);
            }
        }

        e.preventDefault();
    });
}


function toggleUserRole(username, rolename, isAddToRole)
{
    if(gIsDataPosted == false)
    {
        alert(gI18nMessages.AjaxRequestIsBusy);
        return false;
    }

    var isOk = false;
    gIsDataPosted = false;

    var msgOk = "Role " + rolename + " was successfully added to user " + username;
    var msgFail = "Role " + rolename + " was NOT added to user " + username;
    if(isAddToRole == false)
    {
        msgOk = "Role " + rolename + " was successfully removed from user " + username;
        msgFail = "Role " + rolename + " was NOT removed from user " + username;
    }

    jQuery.ajax({
        url: gBaseUrl + "useradmin/dotogglerole/" + username,
        data: "role=" + rolename + "&activate=" + isAddToRole,
        type: "POST",
        dataType: "json",
        success: function(data){
            gIsDataPosted = true;
            console_dump(data, "Response on toggle role:");
            if( data.Status == gStatuses.Approved )
            {
                isOk = true;
                displayMessage(msgOk, "green", 30);
            }else
            {
                displayMessage(msgFail, "red", 30);
            }
        },
        error: function(){
            gIsDataPosted = true;
            displayMessage(msgFail, "red", 30);
            alert(gI18nMessages.AjaxRequestFailed);
        }

    });

    return isOk;
}

var gHideMessageT = null;
function displayMessage(msg, cssClass, hideAfterS)
{
    var oMsg = jQuery("#MessageDiv");
    console_log("Message DIV: " + oMsg);

    if(gHideMessageT != null)
    {
        clearInterval(gHideMessageT);
        gHideMessageT = null;
    }

    console_log("Displaying message: " + msg);
    oMsg.html("<span class='" + cssClass + "'>" + msg + "</span>").show();
    if( hideAfterS > 0 )
    {
        gHideMessageT = setTimeout(function(){
            console_log("Hiding message IDV");
            oMsg.hide();
            gHideMessageT = null;
        }, hideAfterS * 1000);
    }
}

function console_log(msg)
{
    if(gIsConsole)
    {
        console.log(msg);
    }
}

function console_dump(obj, title)
{
    if(gIsConsole)
    {
        if( title != "undefined" )
        {
            console.log(title);
            console.log(obj);
        }
    }
}
