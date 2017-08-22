using System;
using WFEngine.DataAccess;

namespace EnsureDataBaseCreated
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new WorkflowContext())
            {
                context.Database.EnsureCreated();
            }
        }
    }
}