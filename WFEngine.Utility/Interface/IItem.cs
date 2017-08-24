using System;
using System.Collections.Generic;
using System.Text;

namespace WFEngine.Utility.Interface
{
    public interface IItem
    {
        string GetFieldValue(string fieldName);
        string SetFieldValue(string fieldName, string fieldValue);
    }
}
