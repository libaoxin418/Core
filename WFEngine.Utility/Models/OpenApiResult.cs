using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WFEngine.Utility.Models
{
    /// <summary>
    /// api return data
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class OpenApiResult<T>
    {
        public ResultStatus Code { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }

    public enum ResultStatus
    {
        Success = 200,
        Error = 0,
        UnKnow = 404,
        Unauthorized = 401
    }
}
