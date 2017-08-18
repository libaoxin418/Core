using System;
using System.Collections.Generic;
using System.Text;

namespace WFEngine.Utility.Models
{
    public class NodeModel
    {
        /// <summary>
        /// 节点序号
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 节点Id
        /// </summary>
        public Guid Id { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// 节点表单
        /// </summary>
        public FormModel Form { get; set; }

        /// <summary>
        /// 节点审批人
        /// </summary>
        public ApproversModel Approvers { get; set; }

        /// <summary>
        /// 节点按钮
        /// </summary>
        public List<ButtonModel> Buttons { get; set; }

        /// <summary>
        /// 节点字段修改
        /// </summary>
        public FieldsModel Fields { get; set; }
        public int PreNode { get; set; }
        public int NextNode { get; set; }
    }


    public class FormModel
    {
        public bool Item { get; set; }
        public bool Task { get; set; }
        public bool History { get; set; }
    }

    public class ApproversModel
    {
        public List<string> Person { get; set; }
        public List<string> Group { get; set; }
        public List<string> Default { get; set; }
    }

    public class ButtonModel
    {
        public int Index { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class FieldModel
    {
        public string Key { get; set; }
        public object Value { get; set; }
    }

    public class FieldsModel
    {
        public List<FieldModel> SetValue { get; set; }
        public List<FieldModel> GetValue { get; set; }
    }
}
