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
        public string Id { get; set; }
        public string Name { get; set; }
        public string TaskName { get; set; }

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


        public bool IsSendEmail { get; set; }


        public string EmailTmpl { get; set; }
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
        public int PreNode { get; set; }
        public int NextNode { get; set; }
    }
}
