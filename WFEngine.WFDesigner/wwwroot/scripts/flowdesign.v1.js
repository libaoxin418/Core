var FlowDesign = {
    init: function () {
        var workflowId = Utility.GetQueryString("wfId");
        $.ajax({
            url: "/api/workflow/wfDesigner/design?wfid=" + workflowId,
            type: "Get",
            success: function (result) {
                DispWorkflowDesign(result);
            }
        });
    },

    DispWorkflowDesign: function (result) { },

    SaveWorkflowDesign: function () { },


};


var Utility = {
    GetQueryString: function (name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return unescape(r[2]); return null;
    }
}