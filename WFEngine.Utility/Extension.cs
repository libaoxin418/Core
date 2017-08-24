using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using WFEngine.Utility.Models;

namespace WFEngine.Utility
{
    public static class Extension
    {
        public static List<ParameterField> GetParameter(this object input)
        {
            if (input == null)
            {
                return null;
            }

            Regex reg = new Regex("{(\\w*).(\\w*).(\\w*).(\\w*)}");
            MatchCollection matchs = reg.Matches(input.ToString());

            if (matchs.Count < 1)
            {
                return null;
            }

            List<ParameterField> list = new List<ParameterField>();
            foreach (Match m in matchs)
            {
                ParameterField model = new ParameterField();
                model.ReplaceField = m.Groups[0].Value;
                model.Type = m.Groups[1].Value;
                model.Name = m.Groups[4].Value;
                list.Add(model);
            }

            return list;
        }
    }
}
