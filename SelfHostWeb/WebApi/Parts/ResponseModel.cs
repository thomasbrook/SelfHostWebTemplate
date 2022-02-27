using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfHostWeb.WebApi.Parts
{
    public class ResponseModel<T>
    {
        /// <summary>
        /// 实体泛型，状态码为400时，Data为零值
        /// </summary>
        public T Data { get; set; }
        /// <summary>
        /// 200、成功；400、失败（此返回值参照HttpStatus定义，并非HttpStatus）
        /// </summary>
        public int StatusCode { get; set; }
        /// <summary>
        /// 异常信息
        /// </summary>
        public string ExMessage { get; set; }
    }
}
