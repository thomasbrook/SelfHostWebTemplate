using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ApiTemplate
{
    public class TestController : ApiController
    {
        public TestController(string name)
        {

        }

        public dynamic TestStudents(string name, Student stu)
        {
            return new
            {
                result = true,
                desc = "请求成功",
                data = $"你好，{name}。我在另外一个程序集上，不在 webhost 内。"
            };
        }
    }

    public class Student
    {
        public string Name { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public string Age { get; set; }
    }
}
