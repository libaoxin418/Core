var defaults = {
    processData: {},//步骤节点数据
    //processUrl:'',//步骤节点数据
    fnRepeat: function () {
        alert("步骤连接重复");
    },
    fnClick: function () {
        alert("单击");
    },
    fnDbClick: function () {
        alert("双击");
    },
    canvasMenus: {
        "one": function (t) { alert('画面右键') }
    },
    processMenus: {
        "one": function (t) { alert('步骤右键') }
    },
    /*右键菜单样式*/
    menuStyle: {
        border: '1px solid #5a6377',
        minWidth: '150px',
        padding: '5px 0'
    },
    itemStyle: {
        fontFamily: 'verdana',
        color: '#333',
        border: '0',
        /*borderLeft:'5px solid #fff',*/
        padding: '5px 40px 5px 20px'
    },
    itemHoverStyle: {
        border: '0',
        /*borderLeft:'5px solid #49afcd',*/
        color: '#fff',
        backgroundColor: '#5a6377'
    },
    mtAfterDrop: function (params) {
        //alert('连接成功后调用');
        //alert("连接："+params.sourceId +" -> "+ params.targetId);
    },
    //这是连接线路的绘画样式
    connectorPaintStyle: {
        lineWidth: 3,
        strokeStyle: "#49afcd",
        joinstyle: "round"
    },
    //鼠标经过样式
    connectorHoverStyle: {
        lineWidth: 3,
        strokeStyle: "#da4f49"
    }

};
var FlowDesign = {
    initEndPoints: function () {
        $(".generate_line").each(function (i, e) {
            var p = $(e).parent();
            jsPlumb.makeSource($(e), {
                parent: p,
                anchor: "Continuous",
                endpoint: ["Dot", { radius: 1 }],
                connector: ["Flowchart", { stub: [5, 5] }],
                connectorStyle: defaults.connectorPaintStyle,
                hoverPaintStyle: defaults.connectorHoverStyle,
                dragOptions: {},
                maxConnections: -1
            });
        });
    },

    init: function () {
        jsPlumb.importDefaults({
            DragOptions: { cursor: 'pointer' },
            EndpointStyle: { fillStyle: '#225588' },
            Endpoint: ["Dot", { radius: 1 }],
            ConnectionOverlays: [
                ["Arrow", { location: 1 }],
                ["Label", {
                    location: 0.1,
                    id: "label",
                    cssClass: "aLabel"
                }]
            ],
            Anchor: 'Continuous',
            ConnectorZIndex: 5,
            HoverPaintStyle: defaults.connectorHoverStyle
        });
        jsPlumb.setRenderMode(jsPlumb.SVG);

        var workflowId = Utility.GetQueryString("wfId");
        $.ajax({
            url: "/api/workflow/wfDesigner/design?wfid=" + workflowId,
            type: "Get",
            success: function (result) {
                //DispWorkflowDesign(result);
            }
        });
    },

    DispWorkflowDesign: function (result) { },

    SaveWorkflowDesign: function () { },

    AddNode: function (option) {
        var nodeType = option.nodeType;

        var date = new Date();
        var nodeDiv = document.createElement("div");
        var nodeId = "Node" + date.getTime();

        $(nodeDiv).attr("id", nodeId)
            .attr("node_to", "")
            .addClass("wf_node")
            .html("<span class='generate_line " + nodeType + "'></span>" + option.nodeName)
            .dblclick(function () {
                $.get("/templates/Node.html?nodeId=" + nodeId, function (result) {
                    $("#wfdesinger-contaner").append(result);

                    $("#dialog").dialog({ width: 800, height: 400 });
                });
            })
            .mousedown(function (e) {
                if (e.which == 3) { //右键绑定

                }
            });

        $("#flowdesign_canvas").append(nodeDiv);

        jsPlumb.draggable(jsPlumb.getSelector(".wf_node"));
        FlowDesign.initEndPoints();

        //使可以连接线
        jsPlumb.makeTarget(jsPlumb.getSelector(".wf_node"), {
            dropOptions: { hoverClass: "hover", activeClass: "active" },
            anchor: "Continuous",
            maxConnections: -1,
            endpoint: ["Dot", { radius: 1 }],
            paintStyle: { fillStyle: "#ec912a", radius: 1 },
            hoverPaintStyle: this.connectorHoverStyle,
            beforeDrop: function (params) {
                if (params.sourceId === params.targetId) return false;


                //保存节点信息
                return true;
            }
        });
    }
};


var Utility = {
    GetQueryString: function (name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return unescape(r[2]); return null;
    }
}