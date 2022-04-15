using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ApiTemplate.Controller.Parts
{
    public class ResponseModel<T>
    {
        public T Data { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string ExMessage { get; set; }
    }
}
