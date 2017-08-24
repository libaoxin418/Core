using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using WFEngine.Utility.Models;

namespace WFEngine.Utility
{
    public static class Extension
    {
        public static List<ExtField> GetParameter(this object input)
        {
            if (input == null)
            {
                return null;
            }
            List<ExtField> list = new List<ExtField>();

            list.AddRange(GetParameter(input.ToString(), "{(\\w*).(\\w*)}", "DB"));
            list.AddRange(GetParameter(input.ToString(), "[(\\w*).(\\w*)]", "API"));

            return list;
        }

        private static List<ExtField> GetParameter(string input, string pattern, string method)
        {
            Regex reg = new Regex(pattern);
            MatchCollection matchs = reg.Matches(input.ToString());

            if (matchs.Count < 1)
            {
                return null;
            }

            List<ExtField> list = new List<ExtField>();
            foreach (Match m in matchs)
            {
                ExtField model = new ExtField();
                model.ReplaceField = m.Groups[0].Value;
                model.Type = m.Groups[1].Value;
                model.Name = m.Groups[2].Value;
                model.Method = method;
                list.Add(model);
            }

            return list;
        }
    }
}
