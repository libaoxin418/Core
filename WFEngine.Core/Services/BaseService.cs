using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WFEngine.DataAccess;
using WFEngine.DataAccess.Models;

namespace WFEngine.Core.Services
{
    public class BaseService
    {
        protected WorkflowContext _context;

        public virtual void SendEmail(string sendto, string emailTmpl)
        {
            EmailTemplate tmplModel = _context.EmailTemplates.FirstOrDefault(et => et.TmplId == emailTmpl);
        }
    }
}
