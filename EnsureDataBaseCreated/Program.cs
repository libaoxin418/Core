using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using WFEngine.DataAccess;

namespace EnsureDataBaseCreated
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = "{Item.Author}的请加审批{Item.Created}";


            Regex reg = new Regex("{(\\w*).(\\w*)}");
            MatchCollection matchs = reg.Matches(input.ToString());

  
            List<string> list = new List<string>();
            foreach (Match m in matchs)
            {
                list.Add(m.Groups[1].Value);
            }


            //using (var context = new WorkflowContext())
            //{
            //    context.Database.EnsureCreated();
            //}
        }
    }
}