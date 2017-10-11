(function () {

    $("#btnformData").click(function () {
        var formid = Utility.GetQueryString("formid");
        $.get("/templates/formdata.html?formid=" + formid + "&=" + Math.random(), function (result) {
            $("body").append(result);

            $("#close-layer-icon").click(function () {
                $("#formdesigner-pop").remove();
            });
            $("#formdesigner-pop-content").draggable({ handle: "#formdesigner-pop-content-header" });

        });

    });

})();

var Utility = {
    GetQueryString: function (name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return unescape(r[2]); return null;
    },
    WriteLog: function (option) {
        var date = new Date();

        $(".ui-layout-south").prepend("<div class='" + option.Class + "'>" + option.Message + "[" + date.getHours() + ":" + date.getMinutes() + ":" + date.getSeconds() + "]</div>");
    },
    IsInArray: function (arrs, id) {
        for (var i = 0; i < arrs.length; i++) {
            var arr = arrs[i];
            if (arr.Id == id) {
                return true;
            }
        }
        return false;
    },
    getControl: function (name) {
        for (var i = 0; i < controls.length; i++) {
            var ctl = controls[i];
            if (ctl.Name == name) {
                return ctl;
            }
        }

        return null;
    }
}


var controls = [
    {
        Name: "LABEL",
        Content: "{[<label by='topspeed' control='LABEL' property='{\"Value\":\"title\",\"DBField\":\"DB.Table.Field\"}' >Label</label>]}"
    },
    {
        Name: "TEXTBOX",
        Content: "<input by='topspeed' control='TEXTBOX' type='text' property='{\"Width\":\"title\",\"Height\":\"title\",\"DBField\":\"DB.Table.Field\"}' />"
    }
    ,
    {
        Name: "TEXTAREA",
        Content: "<textarea by='topspeed' control='TEXTAREA' DBField=''></textarea>"
    }
    ,
    {
        Name: "RADIO",
        Content: "<input by='topspeed' control='RADIO' type='radio' DBField=''/>"
    }
    ,
    {
        Name: "CHECKBOX",
        Content: "<input by='topspeed' control='CHECKBOX' type='checkbox' DBField=''/>"
    }
    ,
    {
        Name: "BUTTON",
        Content: "<input by='topspeed' control='BUTTON' type='button' value='Button'/>"
    }
    ,
    {
        Name: "SELECT",
        Content: "<select  by='topspeed' control='SELECT' DBField=''></select>"
    }
];