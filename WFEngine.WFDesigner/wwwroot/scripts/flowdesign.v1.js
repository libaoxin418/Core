var defaults = {
    processData: {},//步骤节点数据
    processMenus: {
        "commandAttribute": function (t) {
            var nodeId = $(t).attr("id");
            $.get("/templates/Node.html?nodeId=" + nodeId, { cache: false }, function (result) {
                $("#wfdesinger-contaner").append(result);
                $("#node-tab-content").draggable({ handle: "#node-tab-header" });
            });
        },
        "commandDelete": function (t) {
            $(t).remove();
            var nodes = window.Workflow.Nodes;
            for (var i = 0; i < nodes.length; i++) {
                var node = nodes[i];
                if (t.id == node.Id) {
                    window.Workflow.Nodes.splice(i, 1);
                    break;
                }
            }
            Utility.WriteLog({ "Class": "wf-info", "Message": "节点（" + $(t).text() + "）删除成功！" });
        }
    },
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
var contextmenu = {
    bindings: defaults.processMenus,
    menuStyle: defaults.menuStyle,
    itemStyle: defaults.itemStyle,
    itemHoverStyle: defaults.itemHoverStyle
}
var flowDesign = {
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

    InitCanvas: function () {
        window.Workflow = {
            "WorkflowId": "自动生成",
            "WorkflowName": "未命名",
            "Author": "zhangsan",
            "Created": "2017-08-17",
            "CurrentNode": 0,
            "DataSource": {},
            "Nodes": []
        };

        $(".ui-layout-east").html(JSON.stringify(window.Workflow))
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

        jsPlumb.bind("click", function (c) {
            if (confirm("你确定取消连接吗?")) {
                jsPlumb.detach(c);
                Utility.WriteLog({ "Class": "wf-info", "Message": "连接删除成功！" });
            }
        });

        Utility.WriteLog({ "Class": "wf-info", "Message": "工作流设计器初始化成功！" });
    },

    GetWorkflowDesign: function (workflowId) {
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
        var nodes = window.Workflow.Nodes;

        if (typeof (option.css) == 'undefined') {
            option.css = {};
        }

        var nodeType = option.nodeType;

        var date = new Date();
        var nodeDiv = document.createElement("div");
        var nodeId = "Node" + date.getTime();

        if (nodeType != "node-normal") {
            nodeId = nodeType;
        }

        $(nodeDiv).attr("id", nodeId)
            .attr("node_to", "")
            .css(option.css)
            .addClass("wf_node")
            .html("<span title='点击连线' class='generate_line " + nodeType + "'></span><span class='node_Name'>" + option.nodeName + "</span>")
            .dblclick(function () {
                $.get("/templates/Node.html?nodeId=" + nodeId, { cache: false }, function (result) {
                    $("#wfdesinger-contaner").append(result);
                    $("#node-tab-content").draggable({ handle: "#node-tab-header" });
                });
            })
            .mousedown(function (e) {
                if (e.which == 3) { //右键绑定
                    $(this).contextMenu('NodeMenu', contextmenu);
                }
            });

        $("#flowdesign_canvas").append(nodeDiv);

        jsPlumb.draggable(jsPlumb.getSelector(".wf_node"));
        flowDesign.initEndPoints();

        //使可以连接线
        jsPlumb.makeTarget(jsPlumb.getSelector(".wf_node"), {
            dropOptions: { hoverClass: "hover", activeClass: "active" },
            anchor: "Continuous",
            maxConnections: -1,
            endpoint: ["Dot", { radius: 1 }],
            paintStyle: { fillStyle: "#ec912a", radius: 1 },
            hoverPaintStyle: this.connectorHoverStyle,
            beforeDrop: function (params) {
                if (params.sourceId === params.targetId) {
                    Utility.WriteLog({ "Class": "wf-waring", "Message": "当前工作流版本不允许节点连接自己" });
                    return false
                };

                if (params.sourceId == "node-end") {
                    Utility.WriteLog({ "Class": "wf-waring", "Message": "结束节点不允许连接到其他节点" });
                    return false
                }

                if (params.targetId == "node-start") {
                    Utility.WriteLog({ "Class": "wf-waring", "Message": "不允许其他节点连接到开始节点" });
                    return false
                }

                if (params.sourceId == "node-start" && params.targetId == "node-end") {
                    Utility.WriteLog({ "Class": "wf-waring", "Message": "不允许直接从开始节点连接到结束节点" });
                    return false
                }

                //保存节点信息
                return true;
            }
        });

        var newNode = {};
        newNode.Index = 0;
        newNode.Id = nodeId;
        nodes.push(newNode);

        $(".ui-layout-east").html(JSON.stringify(window.Workflow));

        //记录日志
        Utility.WriteLog({ "Class": "wf-info", "Message": "添加节点：" + option.nodeName });



    },
    Clear: function () {
        jsPlumb.detachEveryConnection();
        jsPlumb.deleteEveryEndpoint();
        jsPlumb.repaintEverything();
    }
};


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
    }
}