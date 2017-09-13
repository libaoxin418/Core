(function () {

    $("#btnformData").click(function () {
        var formid = Utility.GetQueryString("formid");
        $.get("/templates/formdata.html?formid=" + formid, { cache: false }, function (result) {
            $(".ui-layout-center").append(result);

            $("#dialog").dialog({ width: 800, height: 400 });
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
        Properties: [
            { "Name": "Name", "Content": "<input name='Name' type='text' value=''/>" },
            { "Name": "Title", "Content": "<input name='Title' type='text' value=''/>" }
        ],
        Content: "<label by='topspeed' control='LABEL' DBField='' >{Label}</label>",
        SetValue: function (el, option) {
            
        }

    },
    {
        Name: "TEXTBOX",
        Properties: [],
        Content: "<input by='topspeed' control='TEXTBOX' type='text' DBField=''/>"
    }
    ,
    {
        Name: "TEXTAREA",
        Properties: [],
        Content: "<textarea by='topspeed' control='TEXTAREA' DBField=''></textarea>"
    }
    ,
    {
        Name: "RADIO",
        Properties: [],
        Content: "<input by='topspeed' control='RADIO' type='radio' DBField=''/>"
    }
    ,
    {
        Name: "CHECKBOX",
        Properties: [],
        Content: "<input by='topspeed' control='CHECKBOX' type='checkbox' DBField=''/>"
    }
    ,
    {
        Name: "BUTTON",
        Properties: [],
        Content: "<input by='topspeed' control='BUTTON' type='button' value='Button'/>"
    }
    ,
    {
        Name: "SELECT",
        Properties: [],
        Content: "<select  by='topspeed' control='SELECT' DBField=''></select>"
    }
];