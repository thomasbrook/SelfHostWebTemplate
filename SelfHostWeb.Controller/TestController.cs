using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace SelfHostWeb.Controller
{
    public class TestController:ApiController
    {
        public dynamic TestStudents()
        {
            return new
            {
                result = true,
                desc = "请求成功",
                data = "我在另外一个程序集上，不在 webhost 内"
            };
        }
    }
}
