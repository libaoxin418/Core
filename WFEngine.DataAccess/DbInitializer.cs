using System;
using System.Collections.Generic;
using System.Text;

namespace WFEngine.DataAccess
{
    public static class DbInitializer
    {
        public static void Initialize(WorkflowContext context)
        {
            context.Database.EnsureCreated();
        }
    }
}
